using CanaApp.Domain.Contract.Service.File;
using CanaApp.Domain.Contract.Service.User;
using CanaApp.Domain.Entities.Models;
using CancApp.Shared.Abstractions;
using CancApp.Shared.Common.Errors;
using CancApp.Shared.Models.User.ChangePassword;
using CancApp.Shared.Models.User.EditProfile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CanaApp.Application.Services.User
{
    class UserServices(
        UserManager<ApplicationUser> userManager,
        IFileService fileService,
        ILogger<UserServices> logger
        ) : IUserServices
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
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

        public async Task<Result> EditUserProfile(string userId, EditProfileRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result.Failure(UserErrors.UserNotFound);

            if(request.Name is not null)
                user.FullName = request.Name;

            if(request.Address is not null)
                user.Address = request.Address;

            if (request.ImageFile is not null)
                user.Image = await _fileService.SaveFileAsync(request.ImageFile, "profiles");

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) 
                return Result.Success();

            var error = result.Errors.First();

            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));


        }
    }
}
