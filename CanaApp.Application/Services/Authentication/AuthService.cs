using CanaApp.Domain.Contract.Service.Authentication;
using CanaApp.Domain.Contract.Service.File;
using CanaApp.Domain.Entities.Models;
using CancApp.Shared.Abstractions;
using CancApp.Shared.Common.Consts;
using CancApp.Shared.Common.Errors;
using CancApp.Shared.Common.Helpers;
using CancApp.Shared.Models.Authentication;
using CancApp.Shared.Models.Authentication.ConfirmationEmail;
using CancApp.Shared.Models.Authentication.Register;
using CancApp.Shared.Models.Authentication.ResendConfirmationEmail;
using CancApp.Shared.Models.Authentication.ResetPassword;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace CanaApp.Application.Services.Authentication
{
    public class AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtProvider jwtProvider,
        IFileService fileService,
        IHttpContextAccessor httpContextAccessor,
        IEmailSender emailSender,
        ILogger<AuthService> logger) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly IFileService _fileService = fileService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly ILogger<AuthService> _logger = logger;


        private static readonly Dictionary<string, (string Otp, DateTime Expiry)> _otpStore = new();
        private readonly int _otpExpiryMinutes = 15;
        private readonly int _refreshTokenExpiryDays = 14;

        public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            if (await _userManager.FindByEmailAsync(email) is not { } user)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

            if (user.IsDisabled)
                return Result.Failure<AuthResponse>(UserErrors.DisabledUser);

            if ((user.UserType == UserType.Psychiatrist || user.UserType == UserType.Pharmacist
                || user.UserType == UserType.Doctor) && !user.IsConfirmedByAdmin)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

            if(user.IsDisabled || user.NumberOfWarrings >= 5)
            {
                user.IsDisabled = true;
                await _userManager.UpdateAsync(user);
                return Result.Failure<AuthResponse>(UserErrors.DisabledUser);
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, false, true);

            if (result.Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var (token, expiresIn) = _jwtProvider.GenerateToken(user, userRoles);
                var refreshToken = GenerateRefreshToken();
                var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

                user.RefreshTokens.Add(new RefreshToken
                {
                    Token = refreshToken,
                    ExpiresOn = refreshTokenExpiration
                });

                await _userManager.UpdateAsync(user);

                var response = new AuthResponse(user.Id,
                    user.Email,
                    user.UserName!,
                    user.FullName,
                    user.Address!,
                    user.Image,
                    token,
                    expiresIn,
                    refreshToken,
                    refreshTokenExpiration);

                return Result.Success(response);
            }

            var error = result.IsNotAllowed
                ? UserErrors.EmailNotConfirmed
                : result.IsLockedOut
                ? UserErrors.LockedUser
                : UserErrors.InvalidCredentials;

            return Result.Failure<AuthResponse>(error);
        }

        public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwtProvider.ValidateToken(token);

            if (userId is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

            if (user.IsDisabled)
                return Result.Failure<AuthResponse>(UserErrors.DisabledUser);

            if (user.LockoutEnd > DateTime.UtcNow)
                return Result.Failure<AuthResponse>(UserErrors.LockedUser);

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

            if (userRefreshToken is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            var userRoles = await _userManager.GetRolesAsync(user);

            var (newToken, expiresIn) = _jwtProvider.GenerateToken(user, userRoles);
            var newRefreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = newRefreshToken,
                ExpiresOn = refreshTokenExpiration
            });

            await _userManager.UpdateAsync(user);

            var response = new AuthResponse(
                user.Id,
                user.Email,
                user.UserName!,
                user.FullName,
                user.Address!,
                user.Image,
                newToken,
                expiresIn,
                newRefreshToken,
                refreshTokenExpiration);

            return Result.Success(response);
        }

        public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwtProvider.ValidateToken(token);

            if (userId is null)
                return Result.Failure(UserErrors.InvalidJwtToken);

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result.Failure(UserErrors.InvalidJwtToken);

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

            if (userRefreshToken is null)
                return Result.Failure(UserErrors.InvalidRefreshToken);

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return Result.Success();
        }

        public async Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var emailIsExists = await _userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);

            if (emailIsExists)
                return Result.Failure(UserErrors.DuplicatedEmail);

            var user = new ApplicationUser
            {
                Address = request.Address,
                Email = request.Email,
                FullName = request.FullName,
            };
            user.UserType = (UserType)Enum.Parse(typeof(UserType), request.UserType);
            if (request.Image is not null)
            {
                    user.Image = await _fileService.SaveFileAsync(request.Image, "profiles");
            }
            else
            {
                user.Image = "default.png";
            }
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                string otp = GenerateOTP();
                
                var key = $"EMAIL_CONFIRM_{user.Id}";

                _otpStore[key] = (otp, DateTime.UtcNow.AddMinutes(_otpExpiryMinutes));

                _logger.LogInformation("Confirmation otp: {otp}", otp);

                await SendConfirmationEmail(user, otp);

                return Result.Success();
            }

            var error = result.Errors.First();

            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
        public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
        {
            if (await _userManager.FindByIdAsync(request.UserId) is not { } user)
                return Result.Failure(UserErrors.InvalidCode);

            if (user.EmailConfirmed)
                return Result.Failure(UserErrors.DuplicatedConfirmation);

            var key = $"EMAIL_CONFIRM_{user.Id}";

            // Check if OTP exists and is valid
            if (!_otpStore.TryGetValue(key, out var otpInfo) ||
                otpInfo.Otp != request.Code ||
                otpInfo.Expiry < DateTime.UtcNow)
            {
                return Result.Failure(UserErrors.InvalidCode);
            }

            // Remove the OTP from store after usage
            _otpStore.Remove(key);

            // Generate a token for internal use with Identity
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                if(user.UserType == UserType.Patient)
                    await _userManager.AddToRoleAsync(user, DefaultRoles.Patient);
                else if (user.UserType == UserType.Doctor)
                    await _userManager.AddToRoleAsync(user, DefaultRoles.Doctor);
                else if (user.UserType == UserType.Admin)
                    await _userManager.AddToRoleAsync(user, DefaultRoles.Admin);
                else if (user.UserType == UserType.Psychiatrist)
                    await _userManager.AddToRoleAsync(user, DefaultRoles.Psychiatrist);
                else if (user.UserType == UserType.Pharmacist)
                    await _userManager.AddToRoleAsync(user, DefaultRoles.Pharmacist);
                else if (user.UserType == UserType.Volunteer)
                    await _userManager.AddToRoleAsync(user, DefaultRoles.Volunteer);

                return Result.Success();
            }

            var error = result.Errors.First();

            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
        public async Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request)
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
                return Result.Success();

            if (user.EmailConfirmed)
                return Result.Failure(UserErrors.DuplicatedConfirmation);

            string otp = GenerateOTP();

            // Store OTP with expiry time
            var key = $"EMAIL_CONFIRM_{user.Id}";
            _otpStore[key] = (otp, DateTime.UtcNow.AddMinutes(_otpExpiryMinutes));

            _logger.LogInformation("New confirmation OTP for {email}: {otp}", user.Email, otp);

            await SendConfirmationEmail(user, otp);

            return Result.Success();
        }
        public async Task<Result> SendResetPasswordOtpAsync(string email)
        {
            if (await _userManager.FindByEmailAsync(email) is not { } user)
                return Result.Success();

            if (!user.EmailConfirmed)
                return Result.Failure(UserErrors.EmailNotConfirmed);

            string otp = GenerateOTP();

            // Store OTP with expiry time
            var key = $"PWD_RESET_{user.Id}";
            _otpStore[key] = (otp, DateTime.UtcNow.AddMinutes(_otpExpiryMinutes));

            _logger.LogInformation("Password reset OTP for {email}: {otp}", user.Email, otp);

            await SendResetPasswordEmail(user, otp);

            return Result.Success();
        }
        public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null || !user.EmailConfirmed)
                return Result.Failure(UserErrors.InvalidCode);

            var key = $"PWD_RESET_{user.Id}";

            _logger.LogInformation("Password reset OTP for {email}: {otp}", user.Email, request.Otp);
            // Check if OTP exists and is valid
            if (!_otpStore.TryGetValue(key, out var otpInfo) ||
                otpInfo.Otp != request.Otp ||
                otpInfo.Expiry < DateTime.UtcNow)
            {
                return Result.Failure(UserErrors.InvalidCode);
            }

            // Remove the OTP from store after usage
            _otpStore.Remove(key);

            // Generate a token for internal use with Identity
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);


            if (result.Succeeded)
                return Result.Success();

            var error = result.Errors.First();

            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));
        }
        private async Task SendConfirmationEmail(ApplicationUser user, string otp)
        {
            var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
                templateModel: new Dictionary<string, string>
                {
                    { "{{name}}", user.FullName },
                    { "{{otp_code}}", otp }
                }
            );

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "✅ CANC App: Email Confirmation", emailBody));

            await Task.CompletedTask;
        }
        private async Task SendResetPasswordEmail(ApplicationUser user, string otp)
        {
            var emailBody = EmailBodyBuilder.GenerateEmailBody("ForgetPassword",
                templateModel: new Dictionary<string, string>
                {
                    { "{{name}}", user.FullName },
                    { "{{otp_code}}", otp }
                }
            );

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "✅ CancApp: Password Reset", emailBody));

            await Task.CompletedTask;
        }
        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
        private static string GenerateOTP(int length = 4)
        {
            // Generate a numeric OTP of specified length
            Random random = new Random();
            return new string(Enumerable.Repeat(0, length)
                .Select(_ => random.Next(0, 10).ToString()[0])
                .ToArray());
        }
    }
}
