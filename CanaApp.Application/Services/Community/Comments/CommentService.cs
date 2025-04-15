﻿using CanaApp.Domain.Contract.Infrastructure;
using CanaApp.Domain.Contract.Service.Community.Comment;
using CanaApp.Domain.Specification.Comments;
using CancApp.Shared.Abstractions;
using CancApp.Shared.Models.Community.Comments;
using Microsoft.Extensions.Logging;
using CanaApp.Domain.Entities.Comunity;
using CancApp.Shared._Common.Errors;
using AutoMapper;
using CanaApp.Domain.Specification;
using CanaApp.Domain.Specification.Posts;
namespace CanaApp.Application.Services.Community.Comments
{
    internal class CommentService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CommentService> logger
        ) : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CommentService> _logger = logger;


        public async Task<Result<CommentResponse>> GetCommentAsync(int commentId)
        {
            _logger.LogInformation("Getting comment {CommentId}", commentId);
            var spec = new CommentSpecification(c => c.Id == commentId);

            var comment = await _unitOfWork.GetRepository<Comment, int>().GetWithSpecAsync(spec);

            if (comment is null)
            {
                return Result.Failure<CommentResponse>(CommentErrors.CommentNotFound);
            }

            var responsr = _mapper.Map<CommentResponse>(comment);
            return Result.Success(responsr);
        }

        public async Task<Result<IEnumerable<CommentResponse>>> GetCommentsAsync(int postId)
        {
            _logger.LogInformation("Getting comments for post {PostId}", postId);
            var postSpec = new PostSpecification(p => p.Id == postId);
            var post = await _unitOfWork.GetRepository<Post, int>().GetWithSpecAsync(postSpec);
            if (post is null)
            {
                return Result.Failure<IEnumerable<CommentResponse>>(PostErrors.PostNotFound);
            }
            var comments = post.Comments;
            if (comments is null || !comments.Any())
            {
                return Result.Failure<IEnumerable<CommentResponse>>(CommentErrors.CommentNotFound);
            }
            var response = _mapper.Map<IEnumerable<CommentResponse>>(comments);
            return Result.Success(response);
        }
        public async Task<Result> AddCommentAsync(CommentRequest request)
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
            await _unitOfWork.GetRepository<Comment, int>().AddAsync(comment);
            await _unitOfWork.CompleteAsync();

            return Result.Success();
        }

        public Task<Result> DeleteCommentAsync(int commentId)
        {
            throw new NotImplementedException();
        }
        
        public Task<Result> UpdateCommentAsync(CommentRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
