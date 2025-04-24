using CanaApp.Domain.Contract.Infrastructure;
using CanaApp.Domain.Contract.Service.Authentication;
using CanaApp.Domain.Contract.Service.File;
using CanaApp.Domain.Entities.Models;
using CanaApp.Domain.Specification.Models;
using CancApp.Shared.Abstractions;
using CancApp.Shared.Common.Consts;
using CancApp.Shared.Common.Errors;
using CancApp.Shared.Common.Helpers;
using CancApp.Shared.Models.Authentication;
using CancApp.Shared.Models.Authentication.CompleteProfile;
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
        IUnitOfWork unitOfWork,
        IJwtProvider jwtProvider,
        IFileService fileService,
        IHttpContextAccessor httpContextAccessor,
        IEmailSender emailSender,
        ILogger<AuthService> logger) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
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

            

            if (user.UserType == UserType.Volunteer)
            {
                var spec = new VolunteerSpecification(x => x.UserId == user.Id);
                var volunteer = await _unitOfWork.GetRepository<Volunteer, string>().GetWithSpecAsync(spec);
                if (volunteer is null)
                    return Result.Failure<AuthResponse>(UserErrors.UserNotFound);
                if (volunteer.IsDisabled || volunteer.NumberOfWarrings >= 5)
                {
                    if (volunteer.IsDisabled == false)
                    {
                        volunteer.IsDisabled = true;
                        _unitOfWork.GetRepository<Volunteer, string>().Update(volunteer);
                        await _unitOfWork.CompleteAsync();
                    }
                    return Result.Failure<AuthResponse>(UserErrors.DisabledUser);
                }
            }
            else if (user.UserType == UserType.Patient)
            {
                var spec = new PatientSpecification(x => x.UserId == user.Id);
                var patient = await _unitOfWork.GetRepository<Patient, string>().GetWithSpecAsync(spec);
                if (patient is null)
                    return Result.Failure<AuthResponse>(UserErrors.UserNotFound);
                if (patient.IsDisabled || patient.NumberOfWarrings >= 5)
                {
                    if (patient.IsDisabled == false)
                    {
                        patient.IsDisabled = true;
                        _unitOfWork.GetRepository<Patient, string>().Update(patient);
                        await _unitOfWork.CompleteAsync();
                    }
                    return Result.Failure<AuthResponse>(UserErrors.DisabledUser);
                }
            }
            else if (user.UserType == UserType.Doctor)
            {
                var spec = new DoctorSpecification(x => x.UserId == user.Id);
                var doctor = await _unitOfWork.GetRepository<Doctor, string>().GetWithSpecAsync(spec);
                if (doctor is null)
                    return Result.Failure<AuthResponse>(UserErrors.UserNotFound);

                if(!doctor.IsConfirmedByAdmin)
                {
                    return Result.Failure<AuthResponse>(UserErrors.NotCompletedProfile);
                }

                if (doctor.IsDisabled || doctor.NumberOfWarrings >= 5)
                {
                    if (doctor.IsDisabled == false)
                    {
                        doctor.IsDisabled = true;
                        _unitOfWork.GetRepository<Doctor, string>().Update(doctor);
                        await _unitOfWork.CompleteAsync();
                    }
                    return Result.Failure<AuthResponse>(UserErrors.DisabledUser);
                }
            }
            else if (user.UserType == UserType.Psychiatrist)
            {
                var spec = new PsychiatristSpecification(x => x.UserId == user.Id);
                var psychiatrist = await _unitOfWork.GetRepository<Psychiatrist, string>().GetWithSpecAsync(spec);
                if (psychiatrist is null)
                    return Result.Failure<AuthResponse>(UserErrors.UserNotFound);

                if (!psychiatrist.IsConfirmedByAdmin)
                    return Result.Failure<AuthResponse>(UserErrors.NotCompletedProfile);

                if (psychiatrist.IsDisabled || psychiatrist.NumberOfWarrings >= 5)
                {
                    if (psychiatrist.IsDisabled == false)
                    {
                        psychiatrist.IsDisabled = true;
                        _unitOfWork.GetRepository<Psychiatrist, string>().Update(psychiatrist);
                        await _unitOfWork.CompleteAsync();
                    }
                    return Result.Failure<AuthResponse>(UserErrors.DisabledUser);
                }
            }
            else if (user.UserType == UserType.Pharmacist)
            {
                var spec = new PharmacistSpecification(x => x.UserId == user.Id);
                var pharmacist = await _unitOfWork.GetRepository<Pharmacist, string>().GetWithSpecAsync(spec);
               
                if(pharmacist is null)
                    return Result.Failure<AuthResponse>(UserErrors.UserNotFound);

                if (!pharmacist.IsConfirmedByAdmin)
                    return Result.Failure<AuthResponse>(UserErrors.NotCompletedProfile);
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
                    user.UserType.ToString(),
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
            // TODO: check if user is disabled
            //if (user.IsDisabled)
            //    return Result.Failure<AuthResponse>(UserErrors.DisabledUser);

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
                user.UserType.ToString(),
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
                UserName = request.Email,
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
                if(user.UserType == UserType.Patient)
                {
                    await _userManager.AddToRoleAsync(user, DefaultRoles.Patient);
                    var patient = new Patient
                    {
                        UserId = user.Id,
                        NumberOfWarrings = 0,
                        IsDisabled = false,
                        ApplicationUser = user,
                    };
                    await _unitOfWork.GetRepository<Patient, string>().AddAsync(patient);
                    await _unitOfWork.CompleteAsync();
                }
                else if (user.UserType == UserType.Doctor)
                {
                    await _userManager.AddToRoleAsync(user, DefaultRoles.Doctor);

                    var doctor = new Doctor
                    {
                        UserId = user.Id,
                        NumberOfWarrings = 0,
                        IsDisabled = false,
                        ApplicationUser = user,
                        IsConfirmedByAdmin = false,
                        
                    };
                    await _unitOfWork.GetRepository<Doctor, string>().AddAsync(doctor);
                    await _unitOfWork.CompleteAsync();
                }
                else if (user.UserType == UserType.Admin)
                {
                    await _userManager.AddToRoleAsync(user, DefaultRoles.Admin);
                }
                else if (user.UserType == UserType.Psychiatrist)
                {
                    await _userManager.AddToRoleAsync(user, DefaultRoles.Psychiatrist);
                    var psychiatrist = new Psychiatrist
                    {
                        UserId = user.Id,
                        NumberOfWarrings = 0,
                        IsDisabled = false,
                        ApplicationUser = user,
                        IsConfirmedByAdmin = false,                       
                    };
                    await _unitOfWork.GetRepository<Psychiatrist, string>().AddAsync(psychiatrist);
                    await _unitOfWork.CompleteAsync();
                }
                else if (user.UserType == UserType.Pharmacist)
                {
                    await _userManager.AddToRoleAsync(user, DefaultRoles.Pharmacist);
                    var pharmacist = new Pharmacist
                    {
                        UserId = user.Id,
                        NumberOfWarrings = 0,
                        IsDisabled = false,
                        ApplicationUser = user,
                        IsConfirmedByAdmin=false,
                    };
                    await _unitOfWork.GetRepository<Pharmacist, string>().AddAsync(pharmacist);
                    await _unitOfWork.CompleteAsync();

                }
                else if (user.UserType == UserType.Volunteer)
                {
                    await _userManager.AddToRoleAsync(user, DefaultRoles.Volunteer);
                    var volunteer = new Volunteer
                    {
                        UserId = user.Id,
                        NumberOfWarrings = 0,
                        IsDisabled = false,
                        ApplicationUser = user,
                        
                    };
                    await _unitOfWork.GetRepository<Volunteer, string>().AddAsync(volunteer);
                    await _unitOfWork.CompleteAsync();
                }
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
            if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
                return Result.Failure(UserErrors.InvalidCode);

            if (user.EmailConfirmed)
                return Result.Failure(UserErrors.DuplicatedConfirmation);

            var key = $"EMAIL_CONFIRM_{user.Id}";

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

        public  async Task<Result> CompletePharmacyRegistration(CompleteProfilePharmacy completeProfilePharmacy, CancellationToken cancellationToken = default)
        {
            var pharmacistSpec = new PharmacistSpecification(x => x.ApplicationUser.Email == completeProfilePharmacy.Email);
            var pharmacist = await _unitOfWork.GetRepository<Pharmacist, string>().GetWithSpecAsync(pharmacistSpec);

            if (pharmacist is null)
                return Result.Failure(UserErrors.UserNotFound);

            pharmacist.ImageId = await _fileService.SaveFileAsync(completeProfilePharmacy.ImageId, "pharmacies");

            pharmacist.ImagePharmacyLicense = await _fileService.SaveFileAsync(completeProfilePharmacy.ImagePharmacyLicense, "pharmacies");

            pharmacist.NumberOfWorkingHours = completeProfilePharmacy.NumberOfWorkingHours;

            pharmacist.IsDeliveryEnabled = completeProfilePharmacy.IsDeliveryEnabled;

              _unitOfWork.GetRepository<Pharmacist, string>().Update(pharmacist);
            await _unitOfWork.CompleteAsync();

            return Result.Success();
        }
        public async Task<Result> CompleteDoctorRegistration(CompleteProfileDoctor completeProfileDoctor, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(completeProfileDoctor.Email);
            if (user is null ||(user.UserType != UserType.Doctor && user.UserType != UserType.Psychiatrist))
                return Result.Failure(UserErrors.UserNotFound);

            if(user.UserType == UserType.Doctor)
            {
                var doctorSpec = new DoctorSpecification(x => x.ApplicationUser.Email == completeProfileDoctor.Email);

                var doctor = await _unitOfWork.GetRepository<Doctor, string>().GetWithSpecAsync(doctorSpec);

                if (doctor is null)
                    return Result.Failure(UserErrors.UserNotFound);

                doctor.MedicalSyndicatePhoto = await _fileService.SaveFileAsync(completeProfileDoctor.MedicalSyndicatePhoto, "doctors");

                doctor.ImageId = await _fileService.SaveFileAsync(completeProfileDoctor.ImageId, "doctors");

                _unitOfWork.GetRepository<Doctor, string>().Update(doctor);

                await _unitOfWork.CompleteAsync();

            }
            else
            {
                var psychiatristSpec = new PsychiatristSpecification(x => x.ApplicationUser.Email == completeProfileDoctor.Email);

            var psychiatrist = await _unitOfWork.GetRepository<Psychiatrist, string>().GetWithSpecAsync(psychiatristSpec);
            if (psychiatrist is null)
                return Result.Failure(UserErrors.UserNotFound);

            psychiatrist.MedicalSyndicatePhoto = await _fileService.SaveFileAsync(completeProfileDoctor.MedicalSyndicatePhoto, "doctors");
            psychiatrist.ImageId = await _fileService.SaveFileAsync(completeProfileDoctor.ImageId, "doctors");

            _unitOfWork.GetRepository<Psychiatrist, string>().Update(psychiatrist);
            await _unitOfWork.CompleteAsync();

            }
            return Result.Success();
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
