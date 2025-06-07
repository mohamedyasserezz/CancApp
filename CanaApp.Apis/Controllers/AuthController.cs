using CanaApp.Domain.Contract.Service.Authentication;
using CanaApp.Domain.Entities.Models;
using CancApp.Shared.Abstractions;
using CancApp.Shared.Models.Authentication.CompleteProfile;
using CancApp.Shared.Models.Authentication.ConfirmationEmail;
using CancApp.Shared.Models.Authentication.ForgetPassword;
using CancApp.Shared.Models.Authentication.Login;
using CancApp.Shared.Models.Authentication.RefreshToken;
using CancApp.Shared.Models.Authentication.Register;
using CancApp.Shared.Models.Authentication.ResendConfirmationEmail;
using CancApp.Shared.Models.Authentication.ResetPassword;
using Microsoft.AspNetCore.Mvc;

namespace CanaApp.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        IAuthService authService,
        ILogger<AuthController> logger) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ILogger<AuthController> _logger = logger;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Logging with email: {email} and password: {password}", loginRequest.Email, loginRequest.Password);

            var response = await _authService.GetTokenAsync(loginRequest.Email, loginRequest.Password, cancellationToken);

            return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
        }
  
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken)
        {
            var response = await _authService.GetRefreshTokenAsync(refreshTokenRequest.Token, refreshTokenRequest.RefreshToken, cancellationToken);

            return response.IsSuccess ? Ok(response.Value) : response.ToProblem();

        }

        [HttpPost("Revoke-refresh-Token")]
        public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken)
        {
            var response = await _authService.RevokeRefreshTokenAsync(refreshTokenRequest.Token, refreshTokenRequest.RefreshToken, cancellationToken);

            return response.IsSuccess ? Ok() : response.ToProblem();

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.RegisterAsync(request);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.ConfirmEmailAsync(request);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpPost("resend-Confirm-email")]
        public async Task<IActionResult> ResendConfirmEmail([FromBody] ResendConfirmationEmailRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("try to resend email for the user with Email: {Email}", request.Email);

            var response = await _authService.ResendConfirmationEmailAsync(request);

            return response.IsSuccess ? Ok() : response.ToProblem();

        }
        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
        {
            var result = await _authService.SendResetPasswordOtpAsync(request.Email);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [HttpPost("register-otp-for-new-password")]
        public async Task<IActionResult> AssignOtpForPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _authService.AssignOtpForPassword(request);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] AssignNewPassword request)
        {
            var result = await _authService.ResetPasswordAsync(request);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpPost("complete-pharmacy-registration")]
        public async Task<IActionResult> CompletePharmacyRegistration([FromForm] CompleteProfilePharmacy request, CancellationToken cancellationToken)
        {
            var result = await _authService.CompletePharmacyRegistration(request, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [HttpPost("complete-doctor-registration")]
        public async Task<IActionResult> CompleteDoctorRegistration([FromForm] CompleteProfileDoctor request, CancellationToken cancellationToken)
        {
            var result = await _authService.CompleteDoctorRegistration(request, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
    }
}
