namespace CancApp.Shared.Models.Community.Comments
{
        public record CommentResponse(
            int Id,
            string Content,
            int PostId,
            DateTime Time,
            string UserId ,
            string UserImageUrl,
            string Name
    );
}
