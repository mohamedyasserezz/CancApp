using CanaApp.Domain.Contract.Infrastructure;
using CanaApp.Domain.Contract.Service.Authentication;
using CanaApp.Domain.Contract.Service.Dashboard;
using CanaApp.Domain.Contract.Service.File;
using CanaApp.Domain.Entities.Comunity;
using CanaApp.Domain.Entities.Models;
using CanaApp.Domain.Specification;
using CanaApp.Domain.Specification.Community.Comments;
using CanaApp.Domain.Specification.Models;
using CancApp.Shared.Abstractions;
using CancApp.Shared.Common.Consts;
using CancApp.Shared.Common.Errors;
using CancApp.Shared.Common.Helpers;
using CancApp.Shared.Models.Authentication;
using CancApp.Shared.Models.Authentication.CompleteProfile;
using CancApp.Shared.Models.Community.Comments;
using CancApp.Shared.Models.Dashboard;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace CanaApp.Application.Services.Dashboard
{
    class DashboardServices(
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork,
        IFileService fileService,
        SignInManager<ApplicationUser> signInManager,
        IJwtProvider jwtProvider,
        IEmailSender emailSender,
        ILogger<DashboardServices> logger

        ) : IDashboardServices
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFileService _fileService = fileService;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly ILogger<DashboardServices> _logger = logger;



        private readonly int _refreshTokenExpiryDays = 14;

        public async Task<Result<AuthResponse>> Login(string email, string password, CancellationToken cancellationToken = default)
        {
            if (await _userManager.FindByEmailAsync(email) is not { } user)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);



            if(user.UserType != UserType.Admin)
                return Result.Failure<AuthResponse>(UserErrors.Unauthorized);


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
        public async Task<Result> DisableUserAsync(string userId)
        {
            _logger.LogInformation("trying disableUser from DashboardService");
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                _logger.LogInformation("user with Id: {id} not found", userId);
                return Result.Failure(UserErrors.UserNotFound);
            }

            if(user.UserType is UserType.Volunteer or UserType.Patient or UserType.Doctor or UserType.Psychiatrist)
            {
                _logger.LogInformation("Disabling user with id: {id}", userId);

                user.IsDisabled = true;

                _unitOfWork.GetRepository<ApplicationUser, string>().Update(user);

                await _unitOfWork.CompleteAsync();

                return Result.Success();
            }


            return Result.Failure(UserErrors.InvalidRoles);

        }

        public async Task<Result<IEnumerable<CommentResponse>>> GetReportedCommentsAsync()
        {
            _logger.LogInformation("Getting reported comments from DashboardService");
            var commentsSpec = new CommentSpecification(c => c.IsReported);

            var comments = await _unitOfWork.GetRepository<Comment, int>().GetAllWithSpecAsync(commentsSpec);

            var commentsResponse = comments.Select(c => new CommentResponse(
                c.Id,
                c.Content,
                c.PostId,
                c.Time,
                c.UserId,
                _fileService.GetProfileUrl(c.User),
                c.User.FullName,
                c.Reactions.Count
                ));

            return Result.Success(commentsResponse);
        }


        public async Task<Result> AddWarningToUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return Result.Failure(UserErrors.UserNotFound);

            user.NumberOfWarrings++;

            _unitOfWork.GetRepository<ApplicationUser, string>().Update(user);

            await _unitOfWork.CompleteAsync();

            return Result.Success(user);
            
        }

        public async Task<Result> EnableUserAsync(string userId)
        {
            _logger.LogInformation("trying Enable user with id: {id} from DashboardService", userId);
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                _logger.LogInformation("user with Id: {id} not found", userId);
                return Result.Failure(UserErrors.UserNotFound);
            }

            if (user.UserType is UserType.Volunteer or UserType.Patient or UserType.Doctor or UserType.Psychiatrist)
            {
                _logger.LogInformation("Enabling user with id: {id}", userId);
                user.IsDisabled = false;

                _unitOfWork.GetRepository<ApplicationUser, string>().Update(user);

                await _unitOfWork.CompleteAsync();

                return Result.Success();
            }


            return Result.Failure(UserErrors.InvalidRoles);
        }
        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public async Task<Result<NumberOfUsersResponse>> GetUsersChart()
        {

            _logger.LogInformation("Getting users number from DashboardService");
            var userSpec = new Specification<ApplicationUser, string>();

            var userCount = await _unitOfWork.GetRepository<ApplicationUser, string>().GetCountWithSpecAsync(userSpec);

            var patientSpec = new Specification<ApplicationUser, string>(u => u.UserType == UserType.Patient);
            var doctoersSpec = new Specification<ApplicationUser, string>(u => u.UserType == UserType.Doctor);
            var PsychiatristSpec = new Specification<ApplicationUser, string>(u => u.UserType == UserType.Psychiatrist);
            var PharmacistSpec = new Specification<ApplicationUser, string>(u => u.UserType == UserType.Pharmacist);
            var volunteerSpec = new Specification<ApplicationUser, string>(u => u.UserType == UserType.Volunteer);

            var patientsCount = await _unitOfWork.GetRepository<ApplicationUser, string>().GetCountWithSpecAsync(patientSpec);
            var doctorsCount = await _unitOfWork.GetRepository<ApplicationUser, string>().GetCountWithSpecAsync(doctoersSpec);
            var psychiatristCount = await _unitOfWork.GetRepository<ApplicationUser, string>().GetCountWithSpecAsync(PsychiatristSpec);
            var pharmacistCount = await _unitOfWork.GetRepository<ApplicationUser, string>().GetCountWithSpecAsync(PharmacistSpec);
            var volunteerCount = await _unitOfWork.GetRepository<ApplicationUser, string>().GetCountWithSpecAsync(volunteerSpec);

            var response = new NumberOfUsersResponse(
                userCount,
                doctorsCount,
                patientsCount,
                pharmacistCount,
                volunteerCount,
                psychiatristCount
                );

            return Result.Success(response);
        }

        public async Task<Result<IEnumerable<CompleteProfileResponse>>> GetUnCompletedProfile()
        {
            _logger.LogInformation("Getting uncompleted profiles from DashboardService");
            var pharmacistSpec = new PharmacistSpecification(p => !p.IsConfirmedByAdmin && !p.IsCompletedProfileFailed);

            var pharmacists = await _unitOfWork.GetRepository<Pharmacist, string>().GetAllWithSpecAsync(pharmacistSpec);

            var doctorsSpec = new DoctorSpecification(d => !d.IsConfirmedByAdmin && !d.IsCompletedProfileFailed);
            var doctors = await _unitOfWork.GetRepository<Doctor, string>().GetAllWithSpecAsync(doctorsSpec);

            var psychiatristSpec = new PsychiatristSpecification(p => !p.IsConfirmedByAdmin && !p.IsCompletedProfileFailed);
            var psychiatrists = await _unitOfWork.GetRepository<Psychiatrist, string>().GetAllWithSpecAsync(psychiatristSpec);

            var response = pharmacists.Select(p => new CompleteProfileResponse(
                _fileService.GetImageUrl("pharmacies", p.ImageId!),
                _fileService.GetImageUrl("pharmacies", p.ImagePharmacyLicense!),
                Users.Pharmacist,
                p.UserId
                ))
                .Union(doctors.Select(d => new CompleteProfileResponse(
                    _fileService.GetImageUrl("doctors", d.ImageId!),
                    _fileService.GetImageUrl("doctors", d.MedicalSyndicatePhoto!),
                    Users.Doctor,
                    d.UserId
                    ))
                .Union(psychiatrists.Select(p => new CompleteProfileResponse(
                    _fileService.GetImageUrl("doctors", p.ImageId!),
                    _fileService.GetImageUrl("doctors", p.MedicalSyndicatePhoto!),
                    Users.Psychiatrist,
                    p.UserId
                    )))
                );

            return Result.Success(response);
        }

        public async Task<Result> ConfirmCompleteProfile(string userId)
        {
            _logger.LogInformation("Trying to confirm complete profile for user with id: {id}", userId);
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                _logger.LogInformation("User with id: {id} not found", userId);
                return Result.Failure(UserErrors.UserNotFound);
            }
            if(user.UserType == UserType.Doctor)
            {
                var doctorSpec = new DoctorSpecification(d => d.UserId == userId);

                var doctor = await _unitOfWork.GetRepository<Doctor, string>().GetWithSpecAsync(doctorSpec);

                if (doctor is null)
                {
                    _logger.LogInformation("Doctor with userId: {id} not found", userId);
                    return Result.Failure(UserErrors.UserNotFound);
                }
                doctor.IsConfirmedByAdmin = true;
                _unitOfWork.GetRepository<Doctor, string>().Update(doctor);
                await _unitOfWork.CompleteAsync();
            }
            else if(user.UserType == UserType.Psychiatrist)
            {
                _logger.LogInformation("Confirming psychiatrist profile for user with id: {id}", userId);
                var psychiatristSpec = new PsychiatristSpecification(p => p.UserId == userId);
                var psychiatrist = await _unitOfWork.GetRepository<Psychiatrist, string>().GetWithSpecAsync(psychiatristSpec);

                if (psychiatrist is null)
                {
                    _logger.LogInformation("Psychiatrist with userId: {id} not found", userId);
                    return Result.Failure(UserErrors.UserNotFound);
                }

                psychiatrist.IsConfirmedByAdmin = true;
                _unitOfWork.GetRepository<Psychiatrist, string>().Update(psychiatrist);
                await _unitOfWork.CompleteAsync();
            }
            else if(user.UserType == UserType.Pharmacist)
            {
                _logger.LogInformation("Confirming Pharmacist profile for user with id: {id}", userId);

                var pharmcistSpec = new PharmacistSpecification(p => p.UserId == userId);
                var pharmacist = await _unitOfWork.GetRepository<Pharmacist, string>().GetWithSpecAsync(pharmcistSpec);

                if (pharmacist is null)
                {
                    _logger.LogInformation("Pharmacist with userId: {id} not found", userId);
                    return Result.Failure(UserErrors.UserNotFound);
                }

                pharmacist.IsConfirmedByAdmin= true;
                _unitOfWork.GetRepository<Pharmacist, string>().Update(pharmacist);
                await _unitOfWork.CompleteAsync();
            }
            else
            {
                return Result.Failure(UserErrors.InvalidRoles);
            }
            await SendRegistrationConfirmedEmail(user);

            return Result.Success();

        }

        public async Task<Result> FailCompleteProfile(string userId)
        {
            _logger.LogInformation("Trying to fail complete profile for user with id: {id}", userId);

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                _logger.LogInformation("User with id: {id} not found", userId);
                return Result.Failure(UserErrors.UserNotFound);
            }

            if(user.UserType == UserType.Psychiatrist)
            {
                _logger.LogInformation("Failling the registration for psychiatrist with id {userId}", userId);

                var psychatristSpec = new PsychiatristSpecification(p => p.UserId == userId);

                var psychiatrist = await _unitOfWork.GetRepository<Psychiatrist, string>().GetWithSpecAsync(psychatristSpec);

                if (psychiatrist is null)
                {
                    _logger.LogInformation("Psychiatrist with userId: {id} not found", userId);
                    return Result.Failure(UserErrors.UserNotFound);
                }
                psychiatrist.IsCompletedProfileFailed = true;
                _unitOfWork.GetRepository<Psychiatrist, string>().Update(psychiatrist);
                await _unitOfWork.CompleteAsync();

            }
            else if (user.UserType == UserType.Doctor)
            {
                _logger.LogInformation("Falling Doctor profile for user with id: {id}", userId);
                var doctorSpec = new DoctorSpecification(d => d.UserId == userId);

                var doctor = await _unitOfWork.GetRepository<Doctor, string>().GetWithSpecAsync(doctorSpec);

                if (doctor is null)
                {
                    _logger.LogInformation("Doctor with userId: {id} not found", userId);
                    return Result.Failure(UserErrors.UserNotFound);
                }
                doctor.IsCompletedProfileFailed = true;
                _unitOfWork.GetRepository<Doctor, string>().Update(doctor);
                await _unitOfWork.CompleteAsync();
            }
            else if (user.UserType == UserType.Pharmacist)
            {
                _logger.LogInformation("Falling Pharmacist profile for user with id: {id}", userId);

                var pharmcistSpec = new PharmacistSpecification(p => p.UserId == userId);
                var pharmacist = await _unitOfWork.GetRepository<Pharmacist, string>().GetWithSpecAsync(pharmcistSpec);

                if (pharmacist is null)
                {
                    _logger.LogInformation("Pharmacist with userId: {id} not found", userId);
                    return Result.Failure(UserErrors.UserNotFound);
                }

                pharmacist.IsCompletedProfileFailed = true;
                _unitOfWork.GetRepository<Pharmacist, string>().Update(pharmacist);
                await _unitOfWork.CompleteAsync();
            }
            else
            {
                return Result.Failure(UserErrors.InvalidRoles);
            }
            await SendRegistrationRejectedEmail(user);

            _logger.LogInformation("Registration failed for user with id: {id} successfully", userId);

            return Result.Success();
        }
        private async Task SendRegistrationConfirmedEmail(ApplicationUser user)
        {
            var emailBody = EmailBodyBuilder.GenerateEmailBody("RegistrationConfirmed",
                templateModel: new Dictionary<string, string>
                {
                { "{{name}}", user.FullName }
                }
            );

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "✅ CancApp: Registration Confirmed", emailBody));

            await Task.CompletedTask;
        }
        private async Task SendRegistrationRejectedEmail(ApplicationUser user)
        {
            var emailBody = EmailBodyBuilder.GenerateEmailBody("RegistrationRejected",
                templateModel: new Dictionary<string, string>
                {
                { "{{name}}", user.FullName }
                }
            );

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "❌ CancApp: Registration Not Approved", emailBody));

            await Task.CompletedTask;
        }
    }
}
