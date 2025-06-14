using CanaApp.Application.Services.RecordsAccess;
using CanaApp.Domain.Contract.Service.RecordAccess;
using CancApp.Shared.Abstractions;
using CancApp.Shared.Models.RecordAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CanaApp.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestAccessController(
        ILogger<RequestAccessController> logger,
        IRecordAccessServices recordAccessServices
        ) : ControllerBase
    {
        private readonly ILogger<RequestAccessController> _logger = logger;
        private readonly IRecordAccessServices _recordAccessServices = recordAccessServices;

        [HttpPost]
        public async Task<IActionResult> RequestAccess([FromBody] RecordAccessRequest request)
        {
            var result = await _recordAccessServices.RequestRecordAccessAsync(request);
            return result.IsSuccess ? Created() : result.ToProblem();
        }

        [HttpPost("approve-access")]
        public async Task<IActionResult> ApproveAccess([FromBody] ApproveRecordAccessRequest request)
        {
            var result = await _recordAccessServices.ApproveRecordAccessAsync(request, User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [HttpGet("pending-requests")]
        public async Task<IActionResult> GetPendingRequests()
        {
            var patientId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            _logger.LogInformation("API: Get pending record access requests for patient {PatientId}.", patientId);
            var result = await _recordAccessServices.GetPendingRequestsForPatientAsync(patientId);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("can-view-records")]
        public async Task<IActionResult> CanDoctorViewRecords([FromBody] RecordAccessRequest request)
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            _logger.LogInformation("API: Check if doctor {DoctorId} can view records for patient {PatientId}.", doctorId, request.PatientId);
            var result = await _recordAccessServices.CanDoctorViewPatientRecordsAsync(request, doctorId);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}
