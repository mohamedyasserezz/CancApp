namespace CancApp.Shared.Models.Community.Comments
{
    public record CommentResponse(
        DateTime Time,
        string Content,
        string UserId,
        string UserName,
        string UserImage,
        int PostId
    );
}
