using CanaApp.Application.Hups;
using CanaApp.Domain.Contract.Infrastructure;
using CanaApp.Domain.Contract.Service.Community.Reaction;
using CanaApp.Domain.Contract.Service.File;
using CanaApp.Domain.Contract.Service.Notification;
using CanaApp.Domain.Entities.Comunity;
using CanaApp.Domain.Entities.Models;
using CanaApp.Domain.Specification;
using CanaApp.Domain.Specification.Community.Posts;
using CanaApp.Domain.Specification.Community.Reactions;
using CanaApp.Domain.Specification.Models;
using CancApp.Shared.Abstractions;
using CancApp.Shared.Common.Errors;
using CancApp.Shared.Models.Community.Reactions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace CanaApp.Application.Services.Community.Reactions
{
    class ReactionService(
        IUnitOfWork unitOfWork,
        IFileService fileService,
        IHubContext<CommunityHub> hubContext,
        INotificationServices notificationServices,
        ILogger<ReactionService> logger
        ) : IReactionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFileService _fileService = fileService;
        private readonly IHubContext<CommunityHub> _hubContext = hubContext;
        private readonly INotificationServices _notificationServices = notificationServices;
        private readonly ILogger<ReactionService> _logger = logger;

        public async Task<Result<IEnumerable<ReactionResponse>>> GetReactionsAsync(int postId, int? commentId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting reactions for post {PostId} and comment {CommentId}", postId, commentId);

            var postSpect = new PostSpecification(p => p.Id == postId);

            if (await _unitOfWork.GetRepository<Post, int>().GetWithSpecAsync(postSpect) is not { } post)
            {
                return Result.Failure<IEnumerable<ReactionResponse>>(PostErrors.PostNotFound);
            }
            var spec1 = new ReactionSpecification(
                r => r.PostId == postId &&  r.CommentId.HasValue && r.CommentId == commentId);

            var spec2 = new ReactionSpecification(r => r.PostId == postId && !r.CommentId.HasValue);

            var reactions = commentId.HasValue ?
                await _unitOfWork.GetRepository<Reaction, int>().GetAllWithSpecAsync(spec1)
                : await _unitOfWork.GetRepository<Reaction, int>().GetAllWithSpecAsync(spec2);


            var response = reactions.Select(r => new ReactionResponse(
                r.Time,
                r.PostId,
                r.CommentId,
                r.UserId,
                post.User.FullName,
                _fileService.GetProfileUrl(post.User),
                r.CommentId.HasValue ? true : false
                ));

            return Result.Success(response);
        }
        public async Task<Result> AddReactionAsync(ReactionRequest request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Adding reaction for post {PostId} and comment {CommentId}", request.PostId, request.CommentId);
            if (await _unitOfWork.GetRepository<Post, int>().GetByIdAsync(request.PostId, cancellationToken) is null)
            {
                return Result.Failure(PostErrors.PostNotFound);
            }

            var userSpec = new ApplicationUserSpecification(u => u.Id == request.UserId);

            if (await _unitOfWork.GetRepository<ApplicationUser, string>().GetWithSpecAsync(userSpec) is not { } user)
            {
                return Result.Failure(UserErrors.UserNotFound);
            }

            var reactionSpec = new ReactionSpecification(r =>
                r.UserId == request.UserId &&
                r.PostId == request.PostId &&
                (request.IsComment ? r.CommentId == request.CommentId : r.CommentId == null));

            var react = await _unitOfWork.GetRepository<Reaction, int>().GetWithSpecAsync(reactionSpec);
            if (react is not null)
            {
                 _unitOfWork.GetRepository<Reaction, int>().Delete(react);
            }

            if (request.IsComment && request.CommentId.HasValue)
            {
                if (await _unitOfWork.GetRepository<Comment, int>().GetByIdAsync(request.CommentId.Value, cancellationToken) is null)
                {
                    return Result.Failure(CommentErrors.CommentNotFound);
                }
                
            }

            var reaction = new Reaction
            {
                PostId = request.PostId,
                CommentId = request.CommentId,
                UserId = request.UserId,
            };

            await _unitOfWork.GetRepository<Reaction, int>().AddAsync(reaction, cancellationToken);

            await _unitOfWork.CompleteAsync();

            var reactionResponse = new ReactionResponse(
                reaction.Time,
                reaction.PostId,
                reaction.CommentId,
                reaction.UserId,
                reaction.User.FullName,
                _fileService.GetProfileUrl(reaction.User),
                reaction.CommentId.HasValue
                );

            await _hubContext.Clients.Group("Community").SendAsync("ReceiveReactionUpdate", reactionResponse);

            // --- FCM Notification Logic ---
            // If reaction is on a post, notify post owner (if not the reactor)
            if (!request.IsComment)
            {
                var postSpec = new PostSpecification(p => p.Id == request.PostId);
                var post = await _unitOfWork.GetRepository<Post, int>().GetWithSpecAsync(postSpec);
                if (post != null && post.UserId != request.UserId)
                {
                    var userOwnerSpec = new ApplicationUserSpecification(u => u.Id == post.UserId);
                    var postOwner = await _unitOfWork.GetRepository<ApplicationUser, string>().GetWithSpecAsync(userOwnerSpec);
                    if (postOwner != null && postOwner.FcmTokens != null && postOwner.FcmTokens.Count > 0)
                    {
                        var tokens = postOwner.FcmTokens.Select(t => t.Token).Distinct();
                        var title = "New Reaction on Your Post";
                        var body = $"{user.FullName} reacted to your post.";
                        foreach (var token in tokens)
                        {
                            await _notificationServices.SendNotificationAsync(token, title, body);
                            _logger.LogInformation("Sent post reaction notification to user {UserId} (token: {Token})", postOwner.Id, token);
                        }
                    }
                }
            }
            // If reaction is on a comment, notify comment owner (if not the reactor)
            else if (request.IsComment && request.CommentId.HasValue)
            {
                var comment = await _unitOfWork.GetRepository<Comment, int>().GetByIdAsync(request.CommentId.Value, cancellationToken);
                if (comment != null && comment.UserId != request.UserId)
                {
                    var commentOwnerSpec = new ApplicationUserSpecification(u => u.Id == comment.UserId);
                    var commentOwner = await _unitOfWork.GetRepository<ApplicationUser, string>().GetWithSpecAsync(commentOwnerSpec);
                    if (commentOwner != null && commentOwner.FcmTokens != null && commentOwner.FcmTokens.Count > 0)
                    {
                        var tokens = commentOwner.FcmTokens.Select(t => t.Token).Distinct();
                        var title = "New Reaction on Your Comment";
                        var body = $"{user.FullName} reacted to your comment.";
                        foreach (var token in tokens)
                        {
                            await _notificationServices.SendNotificationAsync(token, title, body);
                            _logger.LogInformation("Sent comment reaction notification to user {UserId} (token: {Token})", commentOwner.Id, token);
                        }
                    }
                }
            }


            return Result.Success();
        }
        public async Task<Result> RemoveReactionAsync(ReactionRequest request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Removing reaction for post {PostId} and comment {CommentId}", request.PostId, request.CommentId);
            if (await _unitOfWork.GetRepository<Post, int>().GetByIdAsync(request.PostId, cancellationToken) is null)
            {
                return Result.Failure(PostErrors.PostNotFound);
            }

            var userSpec = new Specification<ApplicationUser, string>(u => u.Id == request.UserId);

            if (await _unitOfWork.GetRepository<ApplicationUser, string>().GetWithSpecAsync(userSpec) is null)
            {
                return Result.Failure(UserErrors.UserNotFound);
            }

            if (request.IsComment && request.CommentId.HasValue)
            {
                if (await _unitOfWork.GetRepository<Comment, int>().GetByIdAsync(request.CommentId.Value, cancellationToken) is null)
                {
                    return Result.Failure(CommentErrors.CommentNotFound);
                }

            }

            var reactionSpec = new ReactionSpecification(
                r => r.PostId == request.PostId && r.UserId == request.UserId && (request.IsComment ? r.CommentId == request.CommentId : !r.CommentId.HasValue));

            var reaction = await _unitOfWork.GetRepository<Reaction, int>().GetWithSpecAsync(reactionSpec);

            if (reaction is null)
                return Result.Failure(ReactionErrors.ReactionNotFound);

            _unitOfWork.GetRepository<Reaction, int>().Delete(reaction);

            await _unitOfWork.CompleteAsync();

            await _hubContext.Clients.Group("Community").SendAsync("ReceiveReactionRemoved", reaction.Id);

            return Result.Success();
        }
    }
}
