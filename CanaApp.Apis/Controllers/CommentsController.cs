using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CancApp.Shared.Abstractions;
using CanaApp.Domain.Contract.Service.Community.Comment;
using CancApp.Shared.Models.Community.Comments;
namespace CanaApp.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController(
        ICommentService commentService,
        ILogger<CommentsController> logger) : ControllerBase
    {
        private readonly ICommentService _commentService = commentService;
        private readonly ILogger<CommentsController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> GetComments([FromRoute] int postId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching comments for post with id: {id}", postId);
            var response = await _commentService.GetCommentsAsync(postId, cancellationToken);
            return response.IsSuccess ? Ok(response) : response.ToProblem();
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromForm] CommentRequest commentRequest, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating comment for post with id: {id}", commentRequest.PostId);
            var response = await _commentService.AddCommentAsync(commentRequest, cancellationToken);
            return response.IsSuccess ? Created() : response.ToProblem();
        }

        [HttpGet("{commentId:int}")]
        public async Task<IActionResult> GetComment([FromRoute] int commentId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching comment with id: {id}", commentId);
            var response = await _commentService.GetCommentAsync(commentId, cancellationToken);
            return response.IsSuccess ? Ok(response) : response.ToProblem();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateComment([FromForm] UpdateCommentRequest commentRequest, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating comment with id: {id}", commentRequest.CommentId);
            var response = await _commentService.UpdateCommentAsync(commentRequest, cancellationToken);
            return response.IsSuccess ? Ok() : response.ToProblem();
        }
        [HttpDelete("{commentId:int}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int commentId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting comment with id: {id}", commentId);
            var response = await _commentService.DeleteCommentAsync(commentId, cancellationToken);
            return response.IsSuccess ? NoContent() : response.ToProblem();
        }
    }
}
