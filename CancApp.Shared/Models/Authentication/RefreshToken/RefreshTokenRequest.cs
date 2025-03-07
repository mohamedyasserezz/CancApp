namespace CancApp.Shared.Models.Authentication.RefreshToken
{
    public record RefreshTokenRequest(
        string Token,
        string RefreshToken
        );
}
