using CancApp.Shared.Models.Community.Reactions;

namespace CancApp.Shared.Models.Community.Post
{
    public record PostResponse(
    int Id,
    DateTime Time,
    string Content,
    string? Image,
    string UserId,
    int CommentsCount,
    int ReactionsCount,
    IEnumerable<ReactionResponse> Reactions
        );
}
