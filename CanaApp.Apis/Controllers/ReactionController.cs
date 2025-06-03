using CanaApp.Domain.Contract.Service.Community.Reaction;
using Microsoft.AspNetCore.Mvc;
using CancApp.Shared.Abstractions;
using CancApp.Shared.Models.Community.Reactions;
namespace CanaApp.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactionController(IReactionService reactionService,
        ILogger<ReactionController> logger) : ControllerBase
    {
        private readonly IReactionService _reactionService = reactionService;
        private readonly ILogger<ReactionController> _logger = logger;

        [HttpGet("{postId}/{commentId?}")]
        public async Task<IActionResult> GetReactions([FromRoute] int postId,[FromRoute] int? commentId,CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching reactions for post with id: {id}", postId);
            var response = await _reactionService.GetReactionsAsync(postId, commentId,cancellationToken);
            return response.IsSuccess ? Ok(response) : response.ToProblem();
        }

        [HttpPost]
        public async Task<IActionResult> AddReaction([FromBody] ReactionRequest reactionRequest, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Adding reaction for post with id: {id}", reactionRequest.PostId);
            var response = await _reactionService.AddReactionAsync(reactionRequest, cancellationToken);
            return response.IsSuccess ? Created() : response.ToProblem();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveReaction([FromBody] ReactionRequest reactionRequest, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Removing reaction for post with id: {id}", reactionRequest.PostId);
            var response = await _reactionService.RemoveReactionAsync(reactionRequest, cancellationToken);
            return response.IsSuccess ? NoContent() : response.ToProblem();
        }

    }
}
