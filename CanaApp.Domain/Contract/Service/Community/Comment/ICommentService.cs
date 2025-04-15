using CancApp.Shared.Abstractions;
using CancApp.Shared.Models.Community.Comments;

namespace CanaApp.Domain.Contract.Service.Community.Comment
{
    public interface ICommentService
    {
        public Task<Result<CommentResponse>> GetCommentAsync(int commentId);
        public Task<Result<IEnumerable<CommentResponse>>> GetCommentsAsync(int postId);
        public Task<Result> AddCommentAsync(CommentRequest request);
        public Task<Result> UpdateCommentAsync(UpdateCommentRequest request);
        public Task<Result> DeleteCommentAsync(int commentId);
    }
}
