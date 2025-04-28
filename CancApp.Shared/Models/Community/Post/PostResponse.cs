using CancApp.Shared.Models.Community.Comments;
using CancApp.Shared.Models.Community.Reactions;

namespace CancApp.Shared.Models.Community.Post
{
    public record PostResponse(
    string Id,
    DateTime Time,
    string Content,
    string? Image,
    string UserId,
    IEnumerable<ReactionResponse> Reactions,
    IEnumerable<CommentResponse> Comments
    );
}
