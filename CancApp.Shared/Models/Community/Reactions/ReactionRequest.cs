namespace CancApp.Shared.Models.Community.Reactions
{
    public record ReactionRequest(
        bool IsComment,
        int PostId,
        int? CommentId,
        string UserId,
        string ReactionType
        );
}
