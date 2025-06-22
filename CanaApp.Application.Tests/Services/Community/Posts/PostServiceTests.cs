using CanaApp.Application.Hups;
using CanaApp.Application.Services.Community.Posts;
using CanaApp.Domain.Contract;
using CanaApp.Domain.Contract.Infrastructure;
using CanaApp.Domain.Contract.Service.Community.Post;
using CanaApp.Domain.Contract.Service.File;
using CanaApp.Domain.Entities.Comunity;
using CanaApp.Domain.Entities.Models;
using CancApp.Shared.Common.Errors;
using CancApp.Shared.Models.Community.Post;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CanaApp.Application.Tests.Services.Community.Posts
{
    public class PostServiceTests
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HybridCache _hybridCache;
        private readonly ILogger<PostService> _logger;
        private readonly IHubContext<CommunityHub> _hubContext;
        private readonly IFileService _fileService;
        private readonly IPostService _postService;

        // Fakes for nested dependencies
        private readonly IGenricRepository<Post, int> _fakePostRepository;
        private readonly IGenricRepository<Reaction, int> _fakeReactionRepository;
        private readonly IClientProxy _fakeClientProxy;
        private readonly IHubClients _fakeHubClients;

        public PostServiceTests()
        {
            // Primary dependencies
            _userManager = A.Fake<UserManager<ApplicationUser>>(o => o.WithArgumentsForConstructor(() => new UserManager<ApplicationUser>(A.Fake<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null)));
            _unitOfWork = A.Fake<IUnitOfWork>();
            _hybridCache = A.Fake<HybridCache>();
            
            // Use real logger instead of fake - logging is infrastructure, not business logic
            _logger = NullLogger<PostService>.Instance;
            
            _hubContext = A.Fake<IHubContext<CommunityHub>>();
            _fileService = A.Fake<IFileService>();

            // Mock repositories returned by UnitOfWork
            _fakePostRepository = A.Fake<IGenricRepository<Post, int>>();
            _fakeReactionRepository = A.Fake<IGenricRepository<Reaction, int>>();
            A.CallTo(() => _unitOfWork.GetRepository<Post, int>()).Returns(_fakePostRepository);
            A.CallTo(() => _unitOfWork.GetRepository<Reaction, int>()).Returns(_fakeReactionRepository);

            // Mock SignalR Hub Context
            _fakeClientProxy = A.Fake<IClientProxy>();
            _fakeHubClients = A.Fake<IHubClients>();
            A.CallTo(() => _hubContext.Clients).Returns(_fakeHubClients);
            A.CallTo(() => _fakeHubClients.Group(A<string>._)).Returns(_fakeClientProxy);

            // Service under test with real implementation and faked dependencies
            _postService = new PostService(_userManager, _unitOfWork, _hybridCache, _logger, _hubContext, _fileService);
        }

        private ApplicationUser CreateFakeUser(string userId = "a1b2c3d4-e5f6-7890-1234-567890abcdef") => 
            new ApplicationUser { Id = userId, FullName = "Test User" };
        
        private Post CreateFakePost(ApplicationUser user, int postId = 1) => 
            new Post { Id = postId, UserId = user?.Id ?? "a1b2c3d4-e5f6-7890-1234-567890abcdef", User = user, Content = "Test Content", Reactions = new List<Reaction>(), Comments = new List<Comment>() };

        // --- GetPostAsync Tests ---

        [Fact]
        public async Task GetPostAsync_WhenPostExists_ReturnsSuccessWithPostResponse()
        {
            // Arrange
            var user = CreateFakeUser();
            var post = CreateFakePost(user);
            A.CallTo(() => _fakePostRepository.GetWithSpecAsync(A<ISpecification<Post, int>>._)).Returns(post);

            // Act
            var result = await _postService.GetPostAsync(post.Id);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Id.Should().Be(post.Id);
        }

        [Fact]
        public async Task GetPostAsync_WhenPostDoesNotExist_ReturnsFailurePostNotFound()
        {
            // Arrange
            A.CallTo(() => _fakePostRepository.GetWithSpecAsync(A<ISpecification<Post, int>>._)).Returns(Task.FromResult<Post?>(null));

            // Act
            var result = await _postService.GetPostAsync(999);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(PostErrors.PostNotFound);
        }


        [Fact]
        public async Task CreatePostAsync_WithValidUser_ReturnsSuccess()
        {
            // Arrange
            var user = CreateFakeUser();
            var request = new PostRequest("New Post", null, user.Id);
            A.CallTo(() => _userManager.FindByIdAsync(user.Id)).Returns(user);

            // Mock the AddAsync method to set the User navigation property on the post
            A.CallTo(() => _fakePostRepository.AddAsync(A<Post>._, A<CancellationToken>._))
                .Invokes((Post post, CancellationToken token) => post.User = user);

            // Act
            var result = await _postService.CreatePostAsync(request);

            // Assert
            result.IsSuccess.Should().BeTrue();
            A.CallTo(() => _fakePostRepository.AddAsync(A<Post>._, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _unitOfWork.CompleteAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _fakeClientProxy.SendCoreAsync(A<string>._, A<object[]>._, A<CancellationToken>._)).MustHaveHappened();
        }

        [Fact]
        public async Task CreatePostAsync_WithNonExistentUser_ReturnsFailureUserNotFound()
        {
            // Arrange
            var request = new PostRequest("New Post", null, "nonexistentuser");
            A.CallTo(() => _userManager.FindByIdAsync("nonexistentuser")).Returns(Task.FromResult<ApplicationUser?>(null));

            // Act
            var result = await _postService.CreatePostAsync(request);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.UserNotFound);
        }
        
        [Fact]
        public async Task CreatePostAsync_WithImage_CallsFileServiceAndSavesImage()
        {
            // Arrange
            var user = CreateFakeUser();
            var fakeImage = A.Fake<IFormFile>();
            var request = new PostRequest("Post with image", fakeImage, user.Id);
            A.CallTo(() => _userManager.FindByIdAsync(user.Id)).Returns(user);
            A.CallTo(() => _fileService.SaveFileAsync(fakeImage, "posts")).Returns("saved_image_name.jpg");

            // Mock the AddAsync method to set the User navigation property on the post
            A.CallTo(() => _fakePostRepository.AddAsync(A<Post>._, A<CancellationToken>._))
                .Invokes((Post post, CancellationToken token) => post.User = user);

            // Act
            var result = await _postService.CreatePostAsync(request);

            // Assert
            result.IsSuccess.Should().BeTrue();
            A.CallTo(() => _fileService.SaveFileAsync(fakeImage, "posts")).MustHaveHappenedOnceExactly();
            A.CallTo(() => _fakePostRepository.AddAsync(A<Post>.That.Matches(p => p.Image == "saved_image_name.jpg"), A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        }


        // --- UpdatePostAsync Tests ---

        [Fact]
        public async Task UpdatePostAsync_WithValidData_ReturnsSuccess()
        {
            // Arrange
            var user = CreateFakeUser();
            var post = CreateFakePost(user);
            var request = new UpdatePostRequest(post.Id, "Updated Content", null, user.Id);
            A.CallTo(() => _userManager.FindByIdAsync(user.Id)).Returns(user);
            A.CallTo(() => _fakePostRepository.GetByIdAsync(post.Id, A<CancellationToken>._)).Returns(post);

            // Act
            var result = await _postService.UpdatePostAsync(request);

            // Assert
            result.IsSuccess.Should().BeTrue();
            A.CallTo(() => _fakePostRepository.Update(A<Post>.That.Matches(p => p.Content == "Updated Content"))).MustHaveHappenedOnceExactly();
            A.CallTo(() => _unitOfWork.CompleteAsync()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task UpdatePostAsync_WithNonExistentPost_ReturnsFailurePostNotFound()
        {
            // Arrange
            var user = CreateFakeUser();
            var request = new UpdatePostRequest(999, "Content", null, user.Id);
            A.CallTo(() => _userManager.FindByIdAsync(user.Id)).Returns(user);
            A.CallTo(() => _fakePostRepository.GetByIdAsync(999, A<CancellationToken>._)).Returns(Task.FromResult<Post?>(null));

            // Act
            var result = await _postService.UpdatePostAsync(request);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(PostErrors.PostNotFound);
        }

        [Fact]
        public async Task UpdatePostAsync_WithNonExistentUser_ReturnsFailureUserNotFound()
        {
            // Arrange
             var request = new UpdatePostRequest(1, "Content", null, "nonexistentuser");
            A.CallTo(() => _userManager.FindByIdAsync("nonexistentuser")).Returns(Task.FromResult<ApplicationUser?>(null));

            // Act
            var result = await _postService.UpdatePostAsync(request);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.UserNotFound);
        }
        
        // --- DeletePostAsync Tests ---

        [Fact]
        public async Task DeletePostAsync_WhenPostExists_ReturnsSuccessAndDeletesReactions()
        {
            // Arrange
            var user = CreateFakeUser();
            var post = CreateFakePost(user);
            var reactions = new List<Reaction> { new Reaction { Id = 1, PostId = post.Id } };
            A.CallTo(() => _fakePostRepository.GetByIdAsync(post.Id, A<CancellationToken>._)).Returns(post);
            A.CallTo(() => _fakeReactionRepository.GetAllWithSpecAsync(A<ISpecification<Reaction, int>>._, A<bool>._)).Returns(reactions);
            
            // Act
            var result = await _postService.DeletePostAsync(post.Id);

            // Assert
            result.IsSuccess.Should().BeTrue();
            A.CallTo(() => _fakeReactionRepository.Delete(A<Reaction>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _fakePostRepository.Delete(post)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _unitOfWork.CompleteAsync()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DeletePostAsync_WhenPostDoesNotExist_ReturnsFailurePostNotFound()
        {
            // Arrange
            A.CallTo(() => _fakePostRepository.GetByIdAsync(999, A<CancellationToken>._)).Returns(Task.FromResult<Post?>(null));

            // Act
            var result = await _postService.DeletePostAsync(999);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(PostErrors.PostNotFound);
        }

        [Fact]
        public async Task DeletePostAsync_WhenPostHasNoReactions_DeletesPostAndReturnsSuccess()
        {
            // Arrange
            var user = CreateFakeUser();
            var post = CreateFakePost(user);
            A.CallTo(() => _fakePostRepository.GetByIdAsync(post.Id, A<CancellationToken>._)).Returns(post);
             // Simulate no reactions found
            A.CallTo(() => _fakeReactionRepository.GetAllWithSpecAsync(A<ISpecification<Reaction, int>>._, A<bool>._)).Returns(new List<Reaction>());

            // Act
            var result = await _postService.DeletePostAsync(post.Id);
            
            // Assert
            result.IsSuccess.Should().BeTrue();
            A.CallTo(() => _fakeReactionRepository.Delete(A<Reaction>._)).MustNotHaveHappened();
            A.CallTo(() => _fakePostRepository.Delete(post)).MustHaveHappenedOnceExactly();
        }
        
        // --- ReportPostAsync Tests ---

        [Fact]
        public async Task ReportPostAsync_WhenPostExists_SetsIsReportedToTrueAndReturnsSuccess()
        {
            // Arrange
            var user = CreateFakeUser();
            var post = CreateFakePost(user);
            post.IsReported = false;
            A.CallTo(() => _fakePostRepository.GetWithSpecAsync(A<ISpecification<Post, int>>._)).Returns(post);

            // Act
            var result = await _postService.ReportPostAsync(post.Id);

            // Assert
            result.IsSuccess.Should().BeTrue();
            A.CallTo(() => _fakePostRepository.Update(A<Post>.That.Matches(p => p.IsReported == true))).MustHaveHappenedOnceExactly();
            A.CallTo(() => _unitOfWork.CompleteAsync()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ReportPostAsync_WhenPostDoesNotExist_ReturnsFailurePostNotFound()
        {
            // Arrange
            A.CallTo(() => _fakePostRepository.GetWithSpecAsync(A<ISpecification<Post, int>>._)).Returns(Task.FromResult<Post?>(null));

            // Act
            var result = await _postService.ReportPostAsync(999);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(PostErrors.PostNotFound);
        }
    }
} 