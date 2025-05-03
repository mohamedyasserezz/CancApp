using CancApp.Shared._Common.Consts;
using CancApp.Shared.Abstractions;
using CancApp.Shared.Models.Community.Post;

namespace CanaApp.Domain.Contract.Service.Community.Post
{
    public interface IPostService
    {
        Task<Result<PostResponse>> GetPostAsync(int postId, CancellationToken cancellationToken = default);
        Task<Result<PaginatedList<PostResponse>>> GetPostsAsync(RequestFilters requestFilters, CancellationToken cancellationToken = default);
        Task<Result> CreatePostAsync(PostRequest postRequest, CancellationToken cancellationToken = default);

        Task<Result> UpdatePostAsync(UpdatePostRequest postRequest, CancellationToken cancellationToken = default);

        Task<Result> DeletePostAsync(int postId, CancellationToken cancellationToken = default);

        Task<Result<IEnumerable<PostResponse>>> GetReportedPosts (CancellationToken cancellationToken = default);

        Task<Result> ReportPostAsync(int postId, CancellationToken cancellationToken = default);

    }
}
