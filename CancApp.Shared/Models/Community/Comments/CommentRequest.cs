namespace CancApp.Shared.Models.Community.Comments
{
    public record class CommentRequest(
        int PostId,
        string UserId,
        string Content
        );
}
