namespace CancApp.Shared.Models.Community.Comments
{
    public record CommentRequest(
        int PostId,
        string UserId,
        string Content
        );
}
