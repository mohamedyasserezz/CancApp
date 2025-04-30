using Microsoft.AspNetCore.Http;

namespace CancApp.Shared.Models.Community.Post
{
    public record UpdatePostRequest
    (
        int Id,
        string? Content,
        IFormFile? Image,
        string UserId
    );
}
