namespace CancApp.Shared.Models.Community.Reactions
{
    public record ReactionResponse(
    DateTime Time,
    int PostId,
    int? CommentId,
    string UserId,
    string UserName,
    string UserProfilePictureUrl,
    string ReactionType,
    bool IsComment
        );
}
