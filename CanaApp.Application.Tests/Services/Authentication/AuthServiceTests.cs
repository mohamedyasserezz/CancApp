using CanaApp.Application.Services.Authentication;
using CanaApp.Domain.Contract.Infrastructure;
using CanaApp.Domain.Contract.Service.Authentication;
using CanaApp.Domain.Contract.Service.File;
using CanaApp.Domain.Entities.Models;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CanaApp.Application.Tests.Services.Authentication
{
    public class AuthServiceTests
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtProvider _jwtProvider;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AuthService> _logger;

        private readonly IAuthService _authService;

        // Helper methods for consistent test data
        private ApplicationUser CreateTestUser(string email, string userId = null!)
        {
            return new ApplicationUser 
            { 
                Id = userId ?? Guid.NewGuid().ToString(), 
                Email = email, 
                UserName = email, 
                FullName = "Test User" 
            };
        }

        public AuthServiceTests()
        {
            // Create fakes for all dependencies
            _userManager = A.Fake<UserManager<ApplicationUser>>(options =>
                options.WithArgumentsForConstructor(() =>
                    new UserManager<ApplicationUser>(A.Fake<IUserStore<ApplicationUser>>(), null!, null!, null!, null!, null!, null!, null!, null!)));

            _signInManager = A.Fake<SignInManager<ApplicationUser>>(options =>
                options.WithArgumentsForConstructor(() =>
                    new SignInManager<ApplicationUser>(_userManager, A.Fake<IHttpContextAccessor>(), A.Fake<IUserClaimsPrincipalFactory<ApplicationUser>>(), null!, null!, null!, null!)));

            _unitOfWork = A.Fake<IUnitOfWork>();
            _jwtProvider = A.Fake<IJwtProvider>();
            _fileService = A.Fake<IFileService>();
            _httpContextAccessor = A.Fake<IHttpContextAccessor>();
            _emailSender = A.Fake<IEmailSender>();
            
            // Use real logger instead of fake - logging is infrastructure, not business logic
            _logger = NullLogger<AuthService>.Instance;

            // Create the service to be tested with real implementation and faked dependencies
            _authService = new AuthService(
                _userManager,
                _signInManager,
                _unitOfWork,
                _jwtProvider,
                _fileService,
                _httpContextAccessor,
                _emailSender,
                _logger);
        }

        [Fact]
        public async Task GetTokenAsync_WithValidCredentials_ReturnsSuccessResultWithAuthResponse()
        {
            // Arrange
            var email = "test@example.com";
            var password = "Password123!";
            var user = CreateTestUser(email);

            A.CallTo(() => _userManager.FindByEmailAsync(email)).Returns(Task.FromResult<ApplicationUser?>(user));

            A.CallTo(() => _signInManager.PasswordSignInAsync(user, password, false, true))
                .Returns(Task.FromResult(SignInResult.Success));
                
            A.CallTo(() => _jwtProvider.GenerateToken(user, A<IList<string>>._))
                .Returns(("fake_token", 3600));

            // Act
            var result = await _authService.GetTokenAsync(email, password, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Token.Should().Be("fake_token");
        }

        [Fact]
        public async Task GetTokenAsync_WithInvalidPassword_ReturnsFailureResult()
        {
            // Arrange
            var email = "test@example.com";
            var password = "WrongPassword!";
            var user = CreateTestUser(email);

            A.CallTo(() => _userManager.FindByEmailAsync(email)).Returns(Task.FromResult<ApplicationUser?>(user));

            A.CallTo(() => _signInManager.PasswordSignInAsync(user, password, false, true))
                .Returns(Task.FromResult(SignInResult.Failed));

            // Act
            var result = await _authService.GetTokenAsync(email, password, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CancApp.Shared.Common.Errors.UserErrors.InvalidCredentials);
        }

        [Fact]
        public async Task GetTokenAsync_WithNonExistentUser_ReturnsFailureResult()
        {
            // Arrange
            var email = "nouser@example.com";
            var password = "anypassword";

            // Configure fake UserManager to find no user
            A.CallTo(() => _userManager.FindByEmailAsync(email)).Returns(Task.FromResult<ApplicationUser?>(null));

            // Act
            var result = await _authService.GetTokenAsync(email, password, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CancApp.Shared.Common.Errors.UserErrors.InvalidCredentials);
        }

        [Fact]
        public async Task GetTokenAsync_WhenUserIsLockedOut_ReturnsFailureResult()
        {
            // Arrange
            var email = "lockedout@example.com";
            var password = "anypassword";
            var user = CreateTestUser(email);

            A.CallTo(() => _userManager.FindByEmailAsync(email)).Returns(Task.FromResult<ApplicationUser?>(user));

            // Configure fake SignInManager to simulate a locked-out user
            A.CallTo(() => _signInManager.PasswordSignInAsync(user, password, false, true))
                .Returns(Task.FromResult(SignInResult.LockedOut));

            // Act
            var result = await _authService.GetTokenAsync(email, password, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CancApp.Shared.Common.Errors.UserErrors.LockedUser);
        }

        [Fact]
        public async Task GetTokenAsync_WhenUserIsDisabled_ReturnsFailureResult()
        {
            // Arrange
            var email = "disabled@example.com";
            var password = "anypassword";
            // Simulate a user who is marked as disabled
            var user = CreateTestUser(email);
            user.IsDisabled = true;
            user.UserType = UserType.Patient;

            A.CallTo(() => _userManager.FindByEmailAsync(email)).Returns(Task.FromResult<ApplicationUser?>(user));

            // Act
            var result = await _authService.GetTokenAsync(email, password, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CancApp.Shared.Common.Errors.UserErrors.DisabledUser);
        }

    
    }
} 