using CanaApp.Application.Services.Dashboard;
using CanaApp.Domain.Contract;
using CanaApp.Domain.Contract.Infrastructure;
using CanaApp.Domain.Contract.Service.Authentication;
using CanaApp.Domain.Contract.Service.Dashboard;
using CanaApp.Domain.Contract.Service.File;
using CanaApp.Domain.Entities.Comunity;
using CanaApp.Domain.Entities.Models;
using CancApp.Shared.Common.Errors;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CanaApp.Application.Tests.Services.Dashboard
{
    public class DashboardServicesTests
    {
        static DashboardServicesTests()
        {
            // Copy email templates to test output directory
            var templatesSourceDir = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "CanaApp.Apis", "Templates");
            var templatesTargetDir = Path.Combine(Directory.GetCurrentDirectory(), "Templates");
            
            if (!Directory.Exists(templatesTargetDir))
                Directory.CreateDirectory(templatesTargetDir);

            var requiredTemplates = new[] { "RegistrationConfirmed.html", "RegistrationRejected.html" };
            
            foreach (var template in requiredTemplates)
            {
                var sourcePath = Path.Combine(templatesSourceDir, template);
                var targetPath = Path.Combine(templatesTargetDir, template);
                
                if (File.Exists(sourcePath))
                    File.Copy(sourcePath, targetPath, true);
            }
        }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtProvider _jwtProvider;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<DashboardServices> _logger;
        private readonly IDashboardServices _dashboardService;

        // Fakes for nested dependencies
        private readonly IGenricRepository<ApplicationUser, string> _fakeUserRepository;
        private readonly IGenricRepository<Comment, int> _fakeCommentRepository;
        private readonly IGenricRepository<Post, int> _fakePostRepository;
        private readonly IGenricRepository<Doctor, string> _fakeDoctorRepository;
        private readonly IGenricRepository<Pharmacist, string> _fakePharmacistRepository;
        private readonly IGenricRepository<Psychiatrist, string> _fakePsychiatristRepository;

        // Helper methods for consistent test data
        private ApplicationUser CreateTestUser(string email, UserType userType = UserType.Admin, string userId = null)
        {
            return new ApplicationUser 
            { 
                Id = userId ?? Guid.NewGuid().ToString(), 
                Email = email, 
                UserName = email, 
                FullName = "Test User",
                UserType = userType,
                IsDisabled = false,
                NumberOfWarrings = 0
            };
        }

        public DashboardServicesTests()
        {
            // Create fakes for all dependencies
            _userManager = A.Fake<UserManager<ApplicationUser>>(options =>
                options.WithArgumentsForConstructor(() =>
                    new UserManager<ApplicationUser>(A.Fake<IUserStore<ApplicationUser>>(), null!, null!, null!, null!, null!, null!, null!, null!)));

            _signInManager = A.Fake<SignInManager<ApplicationUser>>(options =>
                options.WithArgumentsForConstructor(() =>
                    new SignInManager<ApplicationUser>(_userManager, A.Fake<IHttpContextAccessor>(), A.Fake<IUserClaimsPrincipalFactory<ApplicationUser>>(), null!, null!, null!, null!)));

            _unitOfWork = A.Fake<IUnitOfWork>();
            _fileService = A.Fake<IFileService>();
            _jwtProvider = A.Fake<IJwtProvider>();
            _emailSender = A.Fake<IEmailSender>();
            
            // Use real logger instead of fake - logging is infrastructure, not business logic
            _logger = NullLogger<DashboardServices>.Instance;

            // Mock repositories returned by UnitOfWork
            _fakeUserRepository = A.Fake<IGenricRepository<ApplicationUser, string>>();
            _fakeCommentRepository = A.Fake<IGenricRepository<Comment, int>>();
            _fakePostRepository = A.Fake<IGenricRepository<Post, int>>();
            _fakeDoctorRepository = A.Fake<IGenricRepository<Doctor, string>>();
            _fakePharmacistRepository = A.Fake<IGenricRepository<Pharmacist, string>>();
            _fakePsychiatristRepository = A.Fake<IGenricRepository<Psychiatrist, string>>();

            A.CallTo(() => _unitOfWork.GetRepository<ApplicationUser, string>()).Returns(_fakeUserRepository);
            A.CallTo(() => _unitOfWork.GetRepository<Comment, int>()).Returns(_fakeCommentRepository);
            A.CallTo(() => _unitOfWork.GetRepository<Post, int>()).Returns(_fakePostRepository);
            A.CallTo(() => _unitOfWork.GetRepository<Doctor, string>()).Returns(_fakeDoctorRepository);
            A.CallTo(() => _unitOfWork.GetRepository<Pharmacist, string>()).Returns(_fakePharmacistRepository);
            A.CallTo(() => _unitOfWork.GetRepository<Psychiatrist, string>()).Returns(_fakePsychiatristRepository);

            // Create the service to be tested with real implementation and faked dependencies
            _dashboardService = new DashboardServices(
                _userManager,
                _unitOfWork,
                _fileService,
                _signInManager,
                _jwtProvider,
                _emailSender,
                _logger);
        }

        // --- Login Tests ---

        [Fact]
        public async Task Login_WithValidAdminCredentials_ReturnsSuccessResultWithAuthResponse()
        {
            // Arrange
            var email = "admin@example.com";
            var password = "Password123!";
            var user = CreateTestUser(email, UserType.Admin);

            A.CallTo(() => _userManager.FindByEmailAsync(email)).Returns(Task.FromResult<ApplicationUser?>(user));
            A.CallTo(() => _signInManager.PasswordSignInAsync(user, password, false, true))
                .Returns(Task.FromResult(SignInResult.Success));
            A.CallTo(() => _userManager.GetRolesAsync(user)).Returns(Task.FromResult<IList<string>>(new List<string> { "Admin" }));
            A.CallTo(() => _jwtProvider.GenerateToken(user, A<IList<string>>._)).Returns(("fake_token", 3600));
            A.CallTo(() => _fileService.GetProfileUrl(user)).Returns("profile_url");
            A.CallTo(() => _userManager.UpdateAsync(user)).Returns(IdentityResult.Success);

            // Act
            var result = await _dashboardService.Login(email, password);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Token.Should().Be("fake_token");
            result.Value.UserType.Should().Be("Admin");
        }

        [Fact]
        public async Task Login_WithNonAdminUser_ReturnsUnauthorizedError()
        {
            // Arrange
            var email = "user@example.com";
            var password = "Password123!";
            var user = CreateTestUser(email, UserType.Patient);

            A.CallTo(() => _userManager.FindByEmailAsync(email)).Returns(Task.FromResult<ApplicationUser?>(user));

            // Act
            var result = await _dashboardService.Login(email, password);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.Unauthorized);
        }

        [Fact]
        public async Task Login_WithNonExistentUser_ReturnsInvalidCredentialsError()
        {
            // Arrange
            var email = "nouser@example.com";
            var password = "anypassword";

            A.CallTo(() => _userManager.FindByEmailAsync(email)).Returns(Task.FromResult<ApplicationUser?>(null));

            // Act
            var result = await _dashboardService.Login(email, password);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.InvalidCredentials);
        }

        [Fact]
        public async Task Login_WithInvalidPassword_ReturnsInvalidCredentialsError()
        {
            // Arrange
            var email = "admin@example.com";
            var password = "WrongPassword!";
            var user = CreateTestUser(email, UserType.Admin);

            A.CallTo(() => _userManager.FindByEmailAsync(email)).Returns(Task.FromResult<ApplicationUser?>(user));
            A.CallTo(() => _signInManager.PasswordSignInAsync(user, password, false, true))
                .Returns(Task.FromResult(SignInResult.Failed));

            // Act
            var result = await _dashboardService.Login(email, password);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.InvalidCredentials);
        }

        // --- DisableUserAsync Tests ---

        [Fact]
        public async Task DisableUserAsync_WithValidUser_ReturnsSuccess()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var user = CreateTestUser("user@example.com", UserType.Patient, userId);

            A.CallTo(() => _userManager.FindByIdAsync(userId)).Returns(Task.FromResult<ApplicationUser?>(user));

            // Act
            var result = await _dashboardService.DisableUserAsync(userId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            A.CallTo(() => _fakeUserRepository.Update(A<ApplicationUser>.That.Matches(u => u.IsDisabled == true))).MustHaveHappenedOnceExactly();
            A.CallTo(() => _unitOfWork.CompleteAsync()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DisableUserAsync_WithNonExistentUser_ReturnsUserNotFoundError()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            A.CallTo(() => _userManager.FindByIdAsync(userId)).Returns(Task.FromResult<ApplicationUser?>(null));

            // Act
            var result = await _dashboardService.DisableUserAsync(userId);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.UserNotFound);
        }

        [Fact]
        public async Task DisableUserAsync_WithAdminUser_ReturnsInvalidRolesError()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var user = CreateTestUser("admin@example.com", UserType.Admin, userId);

            A.CallTo(() => _userManager.FindByIdAsync(userId)).Returns(Task.FromResult<ApplicationUser?>(user));

            // Act
            var result = await _dashboardService.DisableUserAsync(userId);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.InvalidRoles);
        }

        // --- EnableUserAsync Tests ---

        [Fact]
        public async Task EnableUserAsync_WithValidDisabledUser_ReturnsSuccess()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var user = CreateTestUser("user@example.com", UserType.Patient, userId);
            user.IsDisabled = true;

            A.CallTo(() => _userManager.FindByIdAsync(userId)).Returns(Task.FromResult<ApplicationUser?>(user));

            // Act
            var result = await _dashboardService.EnableUserAsync(userId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            A.CallTo(() => _fakeUserRepository.Update(A<ApplicationUser>.That.Matches(u => u.IsDisabled == false))).MustHaveHappenedOnceExactly();
            A.CallTo(() => _unitOfWork.CompleteAsync()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task EnableUserAsync_WithNonExistentUser_ReturnsUserNotFoundError()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            A.CallTo(() => _userManager.FindByIdAsync(userId)).Returns(Task.FromResult<ApplicationUser?>(null));

            // Act
            var result = await _dashboardService.EnableUserAsync(userId);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.UserNotFound);
        }

        // --- GetReportedCommentsAsync Tests ---

        [Fact]
        public async Task GetReportedCommentsAsync_ReturnsSuccessWithComments()
        {
            // Arrange
            var user = CreateTestUser("user@example.com", UserType.Patient);
            var comments = new List<Comment>
            {
                new Comment { Id = 1, Content = "Reported comment", PostId = 1, UserId = user.Id, User = user, IsReported = true, Reactions = new List<Reaction>() }
            };

            A.CallTo(() => _fakeCommentRepository.GetAllWithSpecAsync(A<ISpecification<Comment, int>>._, A<bool>._)).Returns(comments);
            A.CallTo(() => _fileService.GetProfileUrl(user)).Returns("profile_url");

            // Act
            var result = await _dashboardService.GetReportedCommentsAsync();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Should().HaveCount(1);
        }

        // --- AddWarningToUserAsync Tests ---

        [Fact]
        public async Task AddWarningToUserAsync_WithValidUser_IncrementsWarningCount()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var user = CreateTestUser("user@example.com", UserType.Patient, userId);
            user.NumberOfWarrings = 1;

            A.CallTo(() => _userManager.FindByIdAsync(userId)).Returns(Task.FromResult<ApplicationUser?>(user));

            // Act
            var result = await _dashboardService.AddWarningToUserAsync(userId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            A.CallTo(() => _fakeUserRepository.Update(A<ApplicationUser>.That.Matches(u => u.NumberOfWarrings == 2))).MustHaveHappenedOnceExactly();
            A.CallTo(() => _unitOfWork.CompleteAsync()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task AddWarningToUserAsync_WithNonExistentUser_ReturnsUserNotFoundError()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            A.CallTo(() => _userManager.FindByIdAsync(userId)).Returns(Task.FromResult<ApplicationUser?>(null));

            // Act
            var result = await _dashboardService.AddWarningToUserAsync(userId);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.UserNotFound);
        }

        // --- GetUsersChart Tests ---

        [Fact]
        public async Task GetUsersChart_ReturnsSuccessWithUserCounts()
        {
            // Arrange
            A.CallTo(() => _fakeUserRepository.GetCountWithSpecAsync(A<ISpecification<ApplicationUser, string>>._, A<bool>._)).Returns(100);

            // Act
            var result = await _dashboardService.GetUsersChart();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.NumberOfUsers.Should().Be(100);
        }

        // --- GetUnCompletedProfile Tests ---

        [Fact]
        public async Task GetUnCompletedProfile_ReturnsSuccessWithUncompletedProfiles()
        {
            // Arrange
            var user = CreateTestUser("doctor@example.com", UserType.Doctor);
            var doctor = new Doctor { UserId = user.Id, ImageId = "image1", MedicalSyndicatePhoto = "image2" };

            A.CallTo(() => _fakeDoctorRepository.GetAllWithSpecAsync(A<ISpecification<Doctor, string>>._, A<bool>._)).Returns(new List<Doctor> { doctor });
            A.CallTo(() => _fakePharmacistRepository.GetAllWithSpecAsync(A<ISpecification<Pharmacist, string>>._, A<bool>._)).Returns(new List<Pharmacist>());
            A.CallTo(() => _fakePsychiatristRepository.GetAllWithSpecAsync(A<ISpecification<Psychiatrist, string>>._, A<bool>._)).Returns(new List<Psychiatrist>());
            A.CallTo(() => _fileService.GetImageUrl("doctors", "image1")).Returns("url1");
            A.CallTo(() => _fileService.GetImageUrl("doctors", "image2")).Returns("url2");

            // Act
            var result = await _dashboardService.GetUnCompletedProfile();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Should().HaveCount(1);
        }



        // --- GetReportedPosts Tests ---

        [Fact]
        public async Task GetReportedPosts_ReturnsSuccessWithReportedPosts()
        {
            // Arrange
            var user = CreateTestUser("user@example.com", UserType.Patient);
            var posts = new List<Post>
            {
                new Post { Id = 1, Content = "Reported post", UserId = user.Id, User = user, IsReported = true, Comments = new List<Comment>(), Reactions = new List<Reaction>() }
            };

            A.CallTo(() => _fakePostRepository.GetAllWithSpecAsync(A<ISpecification<Post, int>>._, A<bool>._)).Returns(posts);
            A.CallTo(() => _fileService.GetProfileUrl(user)).Returns("profile_url");

            // Act
            var result = await _dashboardService.GetReportedPosts();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Should().HaveCount(1);
        }

        // --- GetTopPosts Tests ---

        [Fact]
        public async Task GetTopPosts_ReturnsSuccessWithTopPosts()
        {
            // Arrange
            var user = CreateTestUser("user@example.com", UserType.Patient);
            var posts = new List<Post>
            {
                new Post { Id = 1, Content = "Top post", UserId = user.Id, User = user, Comments = new List<Comment>(), Reactions = new List<Reaction>() }
            };

            A.CallTo(() => _fakePostRepository.GetAllWithSpecAsync(A<ISpecification<Post, int>>._, A<bool>._)).Returns(posts);
            A.CallTo(() => _fileService.GetProfileUrl(user)).Returns("profile_url");

            // Act
            var result = await _dashboardService.GetTopPosts(CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Should().HaveCount(1);
        }
    }
} 