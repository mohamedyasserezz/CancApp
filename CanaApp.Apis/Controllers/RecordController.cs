using CanaApp.Domain.Contract.Service.Record;
using CancApp.Shared.Abstractions;
using CancApp.Shared.Models.Record;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CanaApp.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordController(
        IRecordServices recordServices,
        ILogger<ReactionController> logger
        ) : ControllerBase
    {
        private readonly IRecordServices _recordServices = recordServices;
        private readonly ILogger<ReactionController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Getting All record for user {userid}", User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _recordServices.GetAllRecordsAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] RecordRequest request)
        {
            var result = await _recordServices.CreateRecordAsync(request);

            return result.IsSuccess ? Created() : result.ToProblem();
        }
        [HttpPut]
        public async Task<IActionResult> Updated([FromForm] UpdateRecordRequest request)
        {
            var result = await _recordServices.UpdateRecordAsync(request);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Updated([FromRoute] int id)
        {
            var result = await _recordServices.DeleteRecordAsync(id);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }
    }
}
