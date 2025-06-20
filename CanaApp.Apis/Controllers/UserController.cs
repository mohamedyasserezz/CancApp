﻿using CanaApp.Domain.Contract.Service.Notification;
using CanaApp.Domain.Contract.Service.User;
using CancApp.Shared.Abstractions;
using CancApp.Shared.Models.Notification;
using CancApp.Shared.Models.User.ChangePassword;
using CancApp.Shared.Models.User.EditProfile;
using CancApp.Shared.Models.User.Pharmacy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CanaApp.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserServices userServices, ILogger<UserController> logger) : ControllerBase
    {
        private readonly IUserServices _userServices = userServices;
        private readonly ILogger<UserController> _logger = logger;

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            _logger.LogInformation("Received request to change password for user: {UserId}", User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _userServices.ChangePassword(User.FindFirstValue(ClaimTypes.NameIdentifier)!, request);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpPut("EditUserProfile")]
        public async Task<IActionResult> EditProfile([FromForm] EditProfileRequest request)
        {
            _logger.LogInformation($"Edit profile: {request}");
            var result = await _userServices.EditUserProfile(User.FindFirstValue(ClaimTypes.NameIdentifier)!, request);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("pharmacists")]
        public async Task<IActionResult> GetAllPharmacists(GetAllPharmacistRequest request)
        {
            _logger.LogInformation("get all pharmacists");

            var result = await _userServices.GetAllPharmacist(request);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers([FromBody] GetAllUsersRequest request)
        {
            _logger.LogInformation("Get all users with type {type}", request.UserType);
            // Assuming you have a method in IUserServices to get all users
            var result = await _userServices.GetAllUsers(request);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpPost("save-token")]
        public async Task<IActionResult> RegisterToken([FromBody] TokenRequest dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation("API: Register FCM token for user {UserId}", userId);
            var result = await _userServices.SaveTokenAsync(userId, dto.Token);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
    }
}
