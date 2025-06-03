using AutoMapper;
using CanaApp.Domain.Contract.Infrastructure;
using CanaApp.Domain.Contract.Service.Community.Comment;
using CanaApp.Domain.Contract.Service.File;
using CanaApp.Domain.Entities.Comunity;
using CanaApp.Domain.Specification.Community.Comments;
using CanaApp.Domain.Specification.Community.Posts;
using CancApp.Shared.Abstractions;
using CancApp.Shared.Common.Errors;
using CancApp.Shared.Models.Community.Comments;
using Microsoft.Extensions.Logging;
namespace CanaApp.Application.Services.Community.Comments
{
    internal class CommentService(
        IUnitOfWork unitOfWork,
        IFileService fileService,
        ILogger<CommentService> logger
        ) : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFileService _fileService = fileService;
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
            var userImageUrl = _fileService.GetFileUrl(comment.User);

            var response = new CommentResponse(
                comment.Id,
                comment.Content,
                postId,
                comment.Time,
                comment.UserId,
                userImageUrl,
                comment.User!.FullName
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


            // Debug: Log User data
            foreach (var comment in comments)
            {
                _logger.LogInformation("Comment ID: {Id}, UserId: {UserId}, User: {User}, UserName: {UserName}",
                    comment.Id, comment.UserId, comment.User != null ? "Loaded" : "Null",
                    comment.User != null ? comment.User.UserName : "Null");
            }


            var response = comments.Select(c => new CommentResponse(
                c.Id,
                c.Content,
                postId,
                c.Time,
                c.UserId,
                _fileService.GetFileUrl(c.User),
                c.User.FullName
            ));

            return Result.Success(response);
        }
        public async Task<Result> AddCommentAsync(CommentRequest request, CancellationToken cancellationToken = default)
        {

            _logger.LogInformation("Adding comment for post {PostId}", request.PostId);
            var postSpec = new PostSpecification(p => p.Id == request.PostId);
            var post = await _unitOfWork.GetRepository<Post, int>().GetWithSpecAsync(postSpec);

            

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
            return Result.Success();
        }
    }
}
