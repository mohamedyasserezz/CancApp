namespace CancApp.Shared.Models.Community.Reactions
{
    public record ReactionResponse(
    DateTime Time,
    int PostId,
    int? CommentId,
    string UserId,
    string FullName,
    string UserProfilePictureUrl,
    bool IsComment
        );
}
