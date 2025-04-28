using Microsoft.AspNetCore.Http;

namespace CancApp.Shared.Models.Community.Post
{
    public record PostRequest(
        string Content,
        IFormFile? Image,
        string UserId
        );
}
