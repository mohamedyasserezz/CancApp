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

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetComments([FromRoute] int postId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching comments for post with id: {id}", postId);
            var response = await _commentService.GetCommentsAsync(postId, cancellationToken);
            return response.IsSuccess ? Ok(response) : response.ToProblem();
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentRequest commentRequest, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating comment for post with id: {id}", commentRequest.PostId);
            var response = await _commentService.AddCommentAsync(commentRequest, cancellationToken);
            return response.IsSuccess ? Created() : response.ToProblem();
        }

        [HttpGet("{postId:int}/{commentId:int}")]
        public async Task<IActionResult> GetComment([FromRoute] int postId, [FromRoute] int commentId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching comment with id: {id}", commentId);
            var response = await _commentService.GetCommentAsync(postId, commentId, cancellationToken);
            return response.IsSuccess ? Ok(response) : response.ToProblem();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateComment([FromBody] UpdateCommentRequest commentRequest, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating comment with id: {id}", commentRequest.CommentId);
            var response = await _commentService.UpdateCommentAsync(commentRequest, cancellationToken);
            return response.IsSuccess ? Ok() : response.ToProblem();
        }
        [HttpPut("report/{id}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("reporting comment with id: {id}", id);
            var response = await _commentService.ReportCommentAsync(id);
            return response.IsSuccess ? Ok() : response.ToProblem();
        }
        [HttpDelete("{postId:int}/{commentId:int}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int postId,[FromRoute] int commentId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting comment with id: {id}", commentId);
            var response = await _commentService.DeleteCommentAsync(postId, commentId, cancellationToken);
            return response.IsSuccess ? NoContent() : response.ToProblem();
        }
    }
}
