using CancApp.Shared.Abstractions;
using CancApp.Shared.Models.Authentication;
using CancApp.Shared.Models.Authentication.ConfirmationEmail;
using CancApp.Shared.Models.Authentication.Register;
using CancApp.Shared.Models.Authentication.ResendConfirmationEmail;

namespace CanaApp.Domain.Contract.Service.Authentication
{
    public interface IAuthService
    {
        Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
        Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
        Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
         Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);
         Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request);
        Task<Result> SendResetPasswordCodeAsync(string email);

    }
}
