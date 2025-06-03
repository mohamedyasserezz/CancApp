namespace CancApp.Shared.Models.Community.Reactions
{
    public record ReactionRequest(
        int PostId,
        bool IsComment,
        int? CommentId,
        string UserId
        );
}
