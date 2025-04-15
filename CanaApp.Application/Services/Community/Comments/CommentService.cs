using CanaApp.Domain.Contract.Infrastructure;
using CanaApp.Domain.Contract.Service.Community.Comment;
using CanaApp.Domain.Specification.Comments;
using CancApp.Shared.Abstractions;
using CancApp.Shared.Models.Community.Comments;
using Microsoft.Extensions.Logging;
using CanaApp.Domain.Entities.Comunity;
using CancApp.Shared._Common.Errors;
using AutoMapper;
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

        public Task<Result<IEnumerable<CommentResponse>>> GetCommentsAsync(int postId)
        {
            throw new NotImplementedException();
        }
        public Task<Result> AddCommentAsync(CommentRequest request)
        {
            throw new NotImplementedException();
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
