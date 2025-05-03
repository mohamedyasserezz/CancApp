using AutoMapper;
using CanaApp.Domain.Contract.Infrastructure;
using CanaApp.Domain.Contract.Service.Community.Post;
using CanaApp.Domain.Contract.Service.File;
using CanaApp.Domain.Entities.Comunity;
using CanaApp.Domain.Entities.Models;
using CanaApp.Domain.Specification.Community.Posts;
using CancApp.Shared._Common.Consts;
using CancApp.Shared.Abstractions;
using CancApp.Shared.Common.Errors;
using CancApp.Shared.Models.Community.Post;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace CanaApp.Application.Services.Community.Posts
{
    class PostService(
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork,
        HybridCache hybridCache,
        ILogger<PostService> logger,
        IMapper mapper,
        IFileService fileService
        ) : IPostService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly HybridCache _hybridCache = hybridCache;
        private readonly ILogger<PostService> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IFileService _fileService = fileService;

        private const string _cachePrefix = "availablePosts";
        public async Task<Result<PostResponse>> GetPostAsync(int postId, CancellationToken cancellationToken = default)
        {

            _logger.LogInformation("Getting post with id: {id}", postId);

            var postSpec = new PostSpecification(p => p.Id == postId);

            var post = await _unitOfWork.GetRepository<Post, int>().GetWithSpecAsync(postSpec);

            if (post is null)
            {
                _logger.LogWarning("Post with id: {id} not found", postId);
                return Result.Failure<PostResponse>(PostErrors.PostNotFound);
            }
            var postResponse = _mapper.Map<PostResponse>(post);

            _logger.LogInformation("Post with id: {id} found", postId);

            return Result.Success(postResponse);
        }

        public async Task<Result<PaginatedList<PostResponse>>> GetPostsAsync(RequestFilters requestFilters, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting posts with filters: {filters}", requestFilters);

            var cacheKey = $"{_cachePrefix}_{requestFilters.PageNumber}_{requestFilters.PageSize}";

            var posts = await _hybridCache.GetOrCreateAsync<IEnumerable<PostResponse>>(
                cacheKey,
                async cachEntry =>
            {
                var postSpec = new PostSpecification(requestFilters.PageNumber, requestFilters.PageSize);
                var Posts = await _unitOfWork.GetRepository<Post, int>().GetAllWithSpecAsync(postSpec);
                var postsResponse = _mapper.Map<IEnumerable<PostResponse>>(Posts);
                return postsResponse;
            }, new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(5),     
            }
            );

            if (posts is null)
            {
                _logger.LogWarning("Posts not found");
                return Result.Failure<PaginatedList<PostResponse>>(PostErrors.PostNotFound);
            }

            var paginatedPosts = new PaginatedList<PostResponse>(requestFilters.PageNumber, requestFilters.PageSize, posts.Count(), posts);

            _logger.LogInformation("Posts found with filters: {filters}", requestFilters);

            return Result.Success(paginatedPosts);

        }
        public async Task<Result> CreatePostAsync(PostRequest postRequest, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Creating for user with id: {id}", postRequest.UserId);

            if (await _userManager.FindByIdAsync(postRequest.UserId) is null)
            {
                _logger.LogWarning("User with id: {id} not found", postRequest.UserId);
                return Result.Failure(UserErrors.UserNotFound);
            }

            var post = new Post
            {
                UserId = postRequest.UserId,
                Content = postRequest.Content,
            };
            if (postRequest.Image is not null)
            {
                post.Image = await _fileService.SaveFileAsync(postRequest.Image, "posts");
            }

            await _unitOfWork.GetRepository<Post, int>().AddAsync(post);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Post created with id: {id}", post.Id);

            return Result.Success();

        }
        public async Task<Result> UpdatePostAsync(UpdatePostRequest postRequest, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Updating post with id: {id}", postRequest.Id);

            if (await _userManager.FindByIdAsync(postRequest.UserId) is null)
            {
                _logger.LogWarning("User with id: {id} not found", postRequest.UserId);
                return Result.Failure(UserErrors.UserNotFound);
            }

            var post = await _unitOfWork.GetRepository<Post, int>().GetByIdAsync(postRequest.Id);

            if (post is null)
            {
                _logger.LogWarning("Post with id: {id} not found", postRequest.Id);
                return Result.Failure(PostErrors.PostNotFound);
            }

            if (post.Content != postRequest.Content)
            {
                post.Content = postRequest.Content!;
            }
            if (postRequest.Image is not null)
            {
                post.Image = await _fileService.SaveFileAsync(postRequest.Image, "posts");
            }

            _unitOfWork.GetRepository<Post, int>().Update(post);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Post updated with id: {id}", post.Id);

            return Result.Success();

        }

        public async Task<Result> DeletePostAsync(int postId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Deleting post with id: {id}", postId);
            var post = await _unitOfWork.GetRepository<Post, int>().GetByIdAsync(postId);

            if (post is null)
            {
                _logger.LogWarning("Post with id: {id} not found", postId);
                return Result.Failure(PostErrors.PostNotFound);
            }

             _unitOfWork.GetRepository<Post, int>().Delete(post);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Post deleted with id: {id}", postId);

            return Result.Success();
        }

        public async Task<Result<IEnumerable<PostResponse>>> GetReportedPosts(CancellationToken cancellationToken = default)
        {
            var postsSpec = new PostSpecification(p => p.IsReported == true);

            var posts = await _unitOfWork.GetRepository<Post, int>().GetAllWithSpecAsync(postsSpec);

            return Result.Success(_mapper.Map<IEnumerable<PostResponse>>(posts));
        }

        public async Task<Result> ReportPostAsync(int postId, CancellationToken cancellationToken = default)
        {
            var post = await _unitOfWork.GetRepository<Post, int>().GetByIdAsync(postId);

            if (post is null)
            {
                _logger.LogWarning("Post with id: {id} not found", postId);
                return Result.Failure(PostErrors.PostNotFound);
            }

            post.IsReported = true;
            _unitOfWork.GetRepository<Post, int>().Update(post);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Post with id: {id} reported", postId);

            return Result.Success();
        }
    }
}
