using AutoMapper;
using CanaApp.Application.Hups;
using CanaApp.Domain.Contract.Infrastructure;
using CanaApp.Domain.Contract.Service.Community.Post;
using CanaApp.Domain.Contract.Service.File;
using CanaApp.Domain.Entities.Comunity;
using CanaApp.Domain.Entities.Models;
using CanaApp.Domain.Specification.Community.Posts;
using CanaApp.Domain.Specification.Community.Reactions;
using CancApp.Shared._Common.Consts;
using CancApp.Shared.Abstractions;
using CancApp.Shared.Common.Errors;
using CancApp.Shared.Models.Community.Comments;
using CancApp.Shared.Models.Community.Post;
using CancApp.Shared.Models.Community.Reactions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace CanaApp.Application.Services.Community.Posts
{
    class PostService(
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork,
        HybridCache hybridCache,
        ILogger<PostService> logger,
        IHubContext<CommunityHub> hubContext,
        IFileService fileService
        ) : IPostService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly HybridCache _hybridCache = hybridCache;
        private readonly ILogger<PostService> _logger = logger;
        private readonly IHubContext<CommunityHub> _hubContext = hubContext;
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
            var postResponse = new PostResponse(
                postId,
                post.Time,
                post.Content,
                _fileService.GetProfileUrl(post.User),
                post.Image is not null ? _fileService.GetImageUrl("posts", post.Image) : null!,
                post.UserId,
                post.User.FullName,
                post.Comments.Count,
                post.Reactions.Count,
                post.Reactions.Select(r => new ReactionResponse(
                       r.Time,
                       r.PostId,
                       r.CommentId,
                       r.UserId,
                       post.User.FullName,
                       _fileService.GetProfileUrl(post.User),
                       r.CommentId.HasValue ? true : false
                        ))
                );

            _logger.LogInformation("Post with id: {id} found", postId);

            return Result.Success(postResponse);
        }

        public async Task<Result<PaginatedList<PostResponse>>> GetPostsAsync(RequestFilters requestFilters, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting posts with filters: {filters}", requestFilters);

            var cacheKey = $"{_cachePrefix}";

            var posts = await _hybridCache.GetOrCreateAsync<IEnumerable<PostResponse>>(
                cacheKey,
                async cachEntry =>
            {
                var postSpec = new PostSpecification(requestFilters.PageNumber, requestFilters.PageSize);
                var Posts = await _unitOfWork.GetRepository<Post, int>().GetAllWithSpecAsync(postSpec);

                var postsResponse = Posts.Select(p => new PostResponse(
                    p.Id,
                    p.Time,
                    p.Content,
                    _fileService.GetProfileUrl(p.User),
                    p.Image is not null ? _fileService.GetImageUrl("posts", p.Image) : null!,
                    p.UserId,
                    p.User.FullName,
                    p.Comments.Count,
                    p.Reactions.Count,
                    p.Reactions.Select(r => new ReactionResponse(
                       r.Time,
                       r.PostId,
                       r.CommentId,
                       r.UserId,
                       p.User.FullName,
                       _fileService.GetProfileUrl(p.User),
                       r.CommentId.HasValue ? true : false
                        ))
                    ));
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

            // Broadcast new post
            var postResponse = new PostResponse(
                post.Id,
                post.Time,
                post.Content,
                _fileService.GetProfileUrl(post.User),
                post.Image is not null ? _fileService.GetImageUrl("posts", post.Image) : null!,
                post.UserId,
                post.User.FullName,
                post.Comments.Count,
                post.Reactions.Count,
                post.Reactions.Select(r => new ReactionResponse(
                    r.Time,
                    r.PostId,
                    r.CommentId,
                    r.UserId,
                    r.User.FullName,
                    _fileService.GetProfileUrl(r.User),
                    r.CommentId.HasValue
                ))
            );

            await _hubContext.Clients.Group("Community").SendAsync("ReceivePostUpdate", postResponse);

            await _hybridCache.RemoveAsync($"{_cachePrefix}");
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


            // Broadcast updated post
            var postResponse = new PostResponse(
                post.Id,
                post.Time,
                post.Content,
                _fileService.GetProfileUrl(post.User),
                post.Image is not null ? _fileService.GetImageUrl("posts", post.Image) : null!,
                post.UserId,
                post.User.FullName,
                post.Comments.Count,
                post.Reactions.Count,
                post.Reactions.Select(r => new ReactionResponse(
                    r.Time,
                    r.PostId,
                    r.CommentId,
                    r.UserId,
                    r.User.FullName,
                    _fileService.GetProfileUrl(r.User),
                    r.CommentId.HasValue
                ))
            );
            await _hubContext.Clients.Group("Community").SendAsync("ReceivePostUpdate", postResponse);


            await _hybridCache.RemoveAsync($"{_cachePrefix}_*");

            await _hybridCache.RemoveAsync($"{_cachePrefix}");

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
            var reactionSpec = new ReactionSpecification(r => r.PostId == postId);

            var reactions = await _unitOfWork.GetRepository<Reaction, int>()
                        .GetAllWithSpecAsync(reactionSpec);
            if (reactions.Any())
            {
                _logger.LogWarning("Deleting reactions on Post with id: {id}", postId);
                foreach (var reaction in reactions)
                {
                    _logger.LogInformation("Deleting reaction with id: {id}", reaction.Id);
                    _unitOfWork.GetRepository<Reaction, int>().Delete(reaction);
                }
            }
                

             _unitOfWork.GetRepository<Post, int>().Delete(post);

          

             await _unitOfWork.CompleteAsync();

                _logger.LogInformation("Post deleted with id: {id}", postId);

                await _hubContext.Clients.Group("Community").SendAsync("ReceivePostDeleted", postId);

                await _hybridCache.RemoveAsync($"{_cachePrefix}_*");

                await _hybridCache.RemoveAsync($"{_cachePrefix}");
                return Result.Success();
            
            
        }

        public async Task<Result<IEnumerable<PostResponse>>> GetReportedPosts(CancellationToken cancellationToken = default)
        {
            var postsSpec = new PostSpecification(p => p.IsReported == true);

            var posts = await _unitOfWork.GetRepository<Post, int>().GetAllWithSpecAsync(postsSpec);

            var postsResponse = posts.Select(p => new PostResponse(
                    p.Id,
                    p.Time,
                    p.Content,
                    _fileService.GetProfileUrl(p.User),
                    p.Image is not null ? _fileService.GetImageUrl("posts", p.Image) : null!,
                    p.UserId,
                    p.User.FullName,
                    p.Comments.Count,
                    p.Reactions.Count,
                    p.Reactions.Select(r => new ReactionResponse(
                       r.Time,
                       r.PostId,
                       r.CommentId,
                       r.UserId,
                       p.User.FullName,
                       _fileService.GetProfileUrl(p.User),
                       r.CommentId.HasValue ? true : false
                        ))
                    ));

            return Result.Success(postsResponse);
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

            var postResponse = new PostResponse(
                post.Id,
                post.Time,
                post.Content,
                _fileService.GetProfileUrl(post.User),
                post.Image is not null ? _fileService.GetImageUrl("posts", post.Image) : null!,
                post.UserId,
                post.User.FullName,
                post.Comments.Count,
                post.Reactions.Count,
                post.Reactions.Select(r => new ReactionResponse(
                    r.Time,
                    r.PostId,
                    r.CommentId,
                    r.UserId,
                    r.User.FullName,
                    _fileService.GetProfileUrl(r.User),
                    r.CommentId.HasValue
                ))
             );

            await _hubContext.Clients.Group("Community").SendAsync("ReceivePostUpdate", postResponse);



            return Result.Success();
        }
    }
}
