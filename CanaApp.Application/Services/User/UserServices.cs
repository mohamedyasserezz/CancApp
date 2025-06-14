using CanaApp.Domain.Contract.Infrastructure;
using CanaApp.Domain.Contract.Service.File;
using CanaApp.Domain.Contract.Service.User;
using CanaApp.Domain.Entities.Models;
using CanaApp.Domain.Specification.Models;
using CancApp.Shared.Abstractions;
using CancApp.Shared.Common.Errors;
using CancApp.Shared.Models.User.ChangePassword;
using CancApp.Shared.Models.User.EditProfile;
using CancApp.Shared.Models.User.Pharmacy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CanaApp.Application.Services.User
{
    class UserServices(
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork,
        IFileService fileService,
        ILogger<UserServices> logger
        ) : IUserServices
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFileService _fileService = fileService;
        private readonly ILogger<UserServices> _logger = logger;

        public async Task<Result> ChangePassword(string userId, ChangePasswordRequest request)
        {
            _logger.LogInformation("Attempting to change password for user: {UserId}", userId);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", userId);
                return Result.Failure(UserErrors.UserNotFound);
            }

            var isOldPasswordValid = await _userManager.CheckPasswordAsync(user, request.OldPassword);
            if (!isOldPasswordValid)
            {
                _logger.LogWarning("Invalid old password for user: {UserId}", userId);
                return Result.Failure(UserErrors.InvalidPassword);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (result.Succeeded)
            {
                _logger.LogInformation("Password successfully reset for user: {Name}", user.FullName);
                return Result.Success();
            }

            var error = result.Errors.First();
            _logger.LogError("Failed to reset password for user: {Name}. Error: {Error}", user.FullName, error.Description);
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        public async Task<Result<EditProfileResponse>> EditUserProfile(string userId, EditProfileRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result.Failure<EditProfileResponse>(UserErrors.UserNotFound);

            if(request.Name is not null)
                user.FullName = request.Name;

            if(request.Address is not null)
                user.Address = request.Address;

            if (request.ImageFile is not null)
                user.Image = await _fileService.SaveFileAsync(request.ImageFile, "profiles");

            var result = await _userManager.UpdateAsync(user);

            var response = new EditProfileResponse(
                user.FullName,
                _fileService.GetProfileUrl(user),
                user.Address
                );

            if (result.Succeeded) 
                return Result.Success(response);

            var error = result.Errors.First();

            return Result.Failure<EditProfileResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));


        }

        public async Task<Result<IEnumerable<PharmacistResponse>>> GetAllPharmacist(GetAllPharmacistRequest request)
        {
            var pharmacistSpec = new PharmacistSpecification(p => p.IsConfirmedByAdmin);

            var pharmacists = await _unitOfWork.GetRepository<Pharmacist, string>().GetAllWithSpecAsync(pharmacistSpec);

            IEnumerable<Pharmacist> sortedPharmacists = pharmacists;

            if (request.Longitude != null && request.Latitude != null)
            {
                double reqLat = request.Latitude.Value;
                double reqLon = request.Longitude.Value;

                sortedPharmacists = pharmacists
                    .OrderBy(p => CalculateDistance(reqLat, reqLon, p.Latitude, p.Longitude));
            }

            var response = sortedPharmacists.Select(p => new PharmacistResponse(
                p.User.FullName,
                _fileService.GetProfileUrl(p.User),
                p.User.Address,
                p.IsDeliveryEnabled,
                p.Latitude,
                p.Longitude,
                IsPharmacyOpenNow(p.OpeningHour, p.CloseHour)
            ));

            return Result.Success(response);


        }

        private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Radius of the Earth in km
            var dLat = (lat2 - lat1) * Math.PI / 180.0;
            var dLon = (lon2 - lon1) * Math.PI / 180.0;

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180.0) * Math.Cos(lat2 * Math.PI / 180.0) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c; // Distance in km
        }
        private bool IsPharmacyOpenNow(TimeOnly openingHour, TimeOnly closingHour)
        {
            TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now); // 18:56 EEST

            if (openingHour == closingHour)
            {
                return false; // Assume closed if hours are equal (or true for 24-hour)
            }

            if (openingHour < closingHour)
            {
                // Normal case: e.g., 09:00 to 21:00
                return currentTime >= openingHour && currentTime < closingHour;
            }
            else
            {
                // Overnight case: e.g., 22:00 to 06:00
                return currentTime >= openingHour || currentTime < closingHour;
            }
        }
    }
}
