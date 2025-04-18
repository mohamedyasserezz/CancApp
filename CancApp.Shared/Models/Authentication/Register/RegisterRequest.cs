using Microsoft.AspNetCore.Http;

namespace CancApp.Shared.Models.Authentication.Register
{
    public record RegisterRequest(
    string Email,
    string Password,
    string FullName,
    string Address,
    IFormFile? Image,
    string UserType
);
}
