using Microsoft.AspNetCore.Http;

namespace CancApp.Shared.Models.Authentication.Register
{
    public record RegisterRequest(
    string Email,
    string Password,
    string Name,
    string UserName,
    string? Address,

    IFormFile? Image
);
}
