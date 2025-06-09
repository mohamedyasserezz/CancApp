using CanaApp.Domain.Contract.Service.Dashboard;
using CancApp.Shared.Models.Authentication.Login;
using Microsoft.AspNetCore.Mvc;
using CancApp.Shared.Abstractions;
using CancApp.Shared.Models.Dashboard;

namespace CanaApp.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController(
        IDashboardServices dashboardServices,
        ILogger<DashboardController> logger
        ) : ControllerBase
    {
        private readonly IDashboardServices _dashboardServices = dashboardServices;
        private readonly ILogger<DashboardController> _logger = logger;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Logging with email: {email} and password: {password}", loginRequest.Email, loginRequest.Password);

            var result = await _dashboardServices.Login(loginRequest.Email, loginRequest.Password, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpPut("disable")]
        public async Task<IActionResult> Disable([FromBody] DashboardRequest request)
        {
            _logger.LogInformation("Disable user with id: {id}", request.id);

            var result = await _dashboardServices.DisableUserAsync(request.id);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpGet("reported-comments")]
        public async Task<IActionResult> GetReortedComments()
        {
            _logger.LogInformation("Getting reported comments");

            var result = await _dashboardServices.GetReportedCommentsAsync();

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPut("warning")]
        public async Task<IActionResult> AddWarning([FromBody] DashboardRequest request)
        {
            _logger.LogInformation("Add warning to user with id: {id}", request.id);

            var result = await _dashboardServices.AddWarningToUserAsync(request.id);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [HttpPut("enable")]
        public async Task<IActionResult> EnablieUser([FromBody] DashboardRequest request)
        {
            _logger.LogInformation("enabling user with id: {id}", request.id);

            var result = await _dashboardServices.EnableUserAsync(request.id);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [HttpGet("UserCharts")]
        public async Task<IActionResult> UserChart()
        {
            _logger.LogInformation("Getting users number");

            var result = await _dashboardServices.GetUsersChart();

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpGet("UnCompletedProfile")]
        public async Task<IActionResult> GetUnCompletedProfile()
        {
            _logger.LogInformation("Getting uncompleted profiles");
            var result = await _dashboardServices.GetUnCompletedProfile();
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpPut("ConfirmCompleteProfile")]
        public async Task<IActionResult> ConfirmCompleteProfile([FromBody] DashboardRequest request)
        {
            _logger.LogInformation("Confirming complete profile for user with id: {id}", request.id);
            var result = await _dashboardServices.ConfirmCompleteProfile(request.id);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [HttpPut("FailCompleteProfile")]
        public async Task<IActionResult> FailCompleteProfile([FromBody] DashboardRequest request)
        {
            _logger.LogInformation("Failing complete profile for user with id: {id}", request.id);
            var result = await _dashboardServices.FailCompleteProfile(request.id);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
    }
}
