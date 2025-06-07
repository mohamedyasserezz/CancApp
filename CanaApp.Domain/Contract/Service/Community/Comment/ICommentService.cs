using CancApp.Shared.Abstractions;
using CancApp.Shared.Models.Community.Comments;

namespace CanaApp.Domain.Contract.Service.Community.Comment
{
    public interface ICommentService
    {
        public Task<Result<CommentResponse>> GetCommentAsync(int postId, int commentId, CancellationToken cancellationToken = default);
        public Task<Result<IEnumerable<CommentResponse>>> GetCommentsAsync(int postId, CancellationToken cancellationToken = default);
        public Task<Result> AddCommentAsync(CommentRequest request, CancellationToken cancellationToken = default);
        public Task<Result> UpdateCommentAsync(UpdateCommentRequest request, CancellationToken cancellationToken = default);
        public Task<Result> DeleteCommentAsync(int postId, int commentId, CancellationToken cancellationToken = default);

        public Task<Result> ReportCommentAsync(int id);
    }
}
