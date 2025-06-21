using CanaApp.Application.Hups;
using CanaApp.Domain.Contract.Infrastructure;
using CanaApp.Domain.Contract.Service.Community.Comment;
using CanaApp.Domain.Contract.Service.File;
using CanaApp.Domain.Contract.Service.Notification;
using CanaApp.Domain.Entities.Comunity;
using CanaApp.Domain.Entities.Models;
using CanaApp.Domain.Specification;
using CanaApp.Domain.Specification.Community.Comments;
using CanaApp.Domain.Specification.Community.Posts;
using CanaApp.Domain.Specification.Models;
using CancApp.Shared.Abstractions;
using CancApp.Shared.Common.Errors;
using CancApp.Shared.Models.Community.Comments;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
namespace CanaApp.Application.Services.Community.Comments
{
    internal class CommentService(
        IUnitOfWork unitOfWork,
        IFileService fileService,
        IHubContext<CommunityHub> hubContext,
        INotificationServices notificationServices,
        ILogger<CommentService> logger
        ) : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFileService _fileService = fileService;
        private readonly IHubContext<CommunityHub> _hubContext = hubContext;
        private readonly INotificationServices _notificationServices = notificationServices;
        private readonly ILogger<CommentService> _logger = logger;


        public async Task<Result<CommentResponse>> GetCommentAsync(int postId, int commentId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting comment {CommentId}", commentId);

            var postSpec = new PostSpecification(p => p.Id == postId);
            var post = await _unitOfWork.GetRepository<Post, int>().GetWithSpecAsync(postSpec);

            if (post is null)
            {
                return Result.Failure<CommentResponse>(PostErrors.PostNotFound);
            }
            var commentSpec = new CommentSpecification(c => c.PostId == postId && c.Id == commentId);
            var comment = await _unitOfWork.GetRepository<Comment, int>().GetWithSpecAsync(commentSpec);
            if (comment is null)
            {
                return Result.Failure<CommentResponse>(CommentErrors.CommentNotFound);
            }

            //var userSpec = new Specification<ApplicationUser, string>(u => u.Id == comment.UserId);

            //var user = await _unitOfWork.GetRepository<ApplicationUser, string>().GetWithSpecAsync(userSpec);

            //if (user is null) 
            //    return Result.Failure<CommentResponse>(UserErrors.UserNotFound);

            var response = new CommentResponse(
                comment.Id,
                comment.Content,
                postId,
                comment.Time,
                comment.UserId,
                _fileService.GetProfileUrl(comment.User),
                comment.User.FullName,
                comment.Reactions.Count
                );


            return Result.Success(response);
        }

        public async Task<Result<IEnumerable<CommentResponse>>> GetCommentsAsync(int postId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting comments for post {PostId}", postId);
            var postSpec = new PostSpecification(p => p.Id == postId);
            var post = await _unitOfWork.GetRepository<Post, int>().GetWithSpecAsync(postSpec);
            if (post is null)
            {
                return Result.Failure<IEnumerable<CommentResponse>>(PostErrors.PostNotFound);
            }

            var commentSpec = new CommentSpecification(c => c.PostId == postId);
            var comments = await _unitOfWork.GetRepository<Comment, int>().GetAllWithSpecAsync(commentSpec);

            //var userSpec = new Specification<ApplicationUser, string>(u => u.Id == comment.UserId);

            //var user = await _unitOfWork.GetRepository<ApplicationUser, string>().GetWithSpecAsync(userSpec);

            //if (user is null)
            //    return Result.Failure<IEnumerable<CommentResponse>>(UserErrors.UserNotFound);

          


            var response = comments.Select(c => new CommentResponse(
                c.Id,
                c.Content,
                postId,
                c.Time,
                c.UserId,
                _fileService.GetProfileUrl(c.User),
                c.User.FullName,
                c.Reactions.Count   
            ));

            return Result.Success(response);
        }
        public async Task<Result> AddCommentAsync(CommentRequest request, CancellationToken cancellationToken = default)
        {

            _logger.LogInformation("Adding comment for post {PostId}", request.PostId);
            var postSpec = new PostSpecification(p => p.Id == request.PostId);
            var post = await _unitOfWork.GetRepository<Post, int>().GetWithSpecAsync(postSpec);

            var userSpec = new Specification<ApplicationUser, string>(u => u.Id == request.UserId);

            var user = await _unitOfWork.GetRepository<ApplicationUser, string>().GetWithSpecAsync(userSpec);

            if (user is null)
                return Result.Failure(UserErrors.UserNotFound);

            if (post is null)
                return Result.Failure(PostErrors.PostNotFound);


            var comment = new Comment
            {
                Content = request.Content,
                PostId = request.PostId,
                UserId = request.UserId,
            };
            post.Comments.Add(comment);
            await _unitOfWork.GetRepository<Comment, int>().AddAsync(comment, cancellationToken);

            await _unitOfWork.CompleteAsync();


            var commentResponse = new CommentResponse(
                comment.Id,
                comment.Content,
                request.PostId,
                comment.Time,
                comment.UserId,
                _fileService.GetProfileUrl(user),
                comment.User.FullName,
                comment.Reactions.Count
                );

            await _hubContext.Clients.Group("Community").SendAsync("ReceiveCommentUpdate", commentResponse);

            // --- FCM Notification Logic ---
            // Only notify the post owner if the commenter is not the owner
            if (post.UserId != request.UserId)
            {
                var postOwnerSpec = new ApplicationUserSpecification(u => u.Id == post.UserId);
                var postOwner = await _unitOfWork.GetRepository<ApplicationUser, string>().GetWithSpecAsync(postOwnerSpec);
                if (postOwner != null && postOwner.FcmTokens != null && postOwner.FcmTokens.Count > 0)
                {
                    var tokens = postOwner.FcmTokens.Select(t => t.Token).Distinct();
                    var title = "New Comment on Your Post";
                    var body = $"{user.FullName} commented: \"{comment.Content}\"";
                    foreach (var token in tokens)
                    {
                        await _notificationServices.SendNotificationAsync(token, title, body);
                        _logger.LogInformation("Sent comment notification to user {UserId} (token: {Token})", postOwner.Id, token);
                    }
                }
            }

            return Result.Success();
        }

        public async Task<Result> DeleteCommentAsync(int postId, int commentId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Deleting comment {CommentId}", commentId);
            var postSpec = new PostSpecification(p => p.Id == postId);
            var post = await _unitOfWork.GetRepository<Post, int>().GetWithSpecAsync(postSpec);

            if (post is null)
            {
                return Result.Failure<CommentResponse>(PostErrors.PostNotFound);
            }
            if (post.Comments.Where(c => c.Id == commentId).FirstOrDefault() is not { } comment)
            {
                return Result.Failure<CommentResponse>(CommentErrors.CommentNotFound);
            }

            _unitOfWork.GetRepository<Comment, int>().Delete(comment);
            await _unitOfWork.CompleteAsync();

            await _hubContext.Clients.Group("Community").SendAsync("ReceiveCommentDeleted", commentId);

            return Result.Success();

        }

        public async Task<Result> UpdateCommentAsync(UpdateCommentRequest request, CancellationToken cancellationToken = default)
        {
            var spec = new CommentSpecification(c => c.Id == request.CommentId);
            var comment = await _unitOfWork.GetRepository<Comment, int>().GetWithSpecAsync(spec);

            if (comment is null)
            {
                return Result.Failure(CommentErrors.CommentNotFound);
            }
            comment.Content = request.Content;
            comment.Time = DateTime.UtcNow;


            _unitOfWork.GetRepository<Comment, int>().Update(comment);
            await _unitOfWork.CompleteAsync();

            var commentResponse = new CommentResponse(
                comment.Id,
                comment.Content,
                comment.PostId,
                comment.Time,
                comment.UserId,
                _fileService.GetProfileUrl(comment.User),
                comment.User.FullName,
                comment.Reactions.Count
                );

            await _hubContext.Clients.Group("Community").SendAsync("ReceiveCommentUpdate", commentResponse);
            return Result.Success();
        }

        public async Task<Result> ReportCommentAsync(int id)
        {
            var commentSpec = new CommentSpecification(c => c.Id == id);

            var comment = await _unitOfWork.GetRepository<Comment, int>().GetWithSpecAsync(commentSpec);

            if (comment is null)
                return Result.Failure(CommentErrors.CommentNotFound);

            comment.IsReported = true;

            _unitOfWork.GetRepository<Comment, int>().Update(comment);

            await _unitOfWork.CompleteAsync();

            return Result.Success();
        }
    }
}
