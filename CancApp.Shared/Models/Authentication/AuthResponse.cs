namespace CancApp.Shared.Models.Authentication
{
    public record AuthResponse(
    string Id,
    string? Email,
    string UserName,
    string Name,
    string Address,
    string? Image,
    string Token,
    int ExpiresIn,
    string RefreshToken,
    DateTime RefreshTokenExpiration
);
}
