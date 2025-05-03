using CanaApp.Domain.Contract.Service.Community.Post;
using CancApp.Shared._Common.Consts;
using CancApp.Shared.Models.Community.Post;
using CancApp.Shared.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CanaApp.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController(
        IPostService postService,
        ILogger<PostController> logger) : ControllerBase
    {
        private readonly IPostService postService = postService;
        private readonly ILogger<PostController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> GetPosts([FromRoute] RequestFilters filters, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching posts in page Number: {pageNumber} when page Size: {pageSize}", filters.PageNumber, filters.PageSize);
            var response = await postService.GetPostsAsync(filters, cancellationToken);
            return response.IsSuccess ? Ok(response) : response.ToProblem();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromForm] PostRequest postRequest, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating post for user with id: {id}", postRequest.UserId);
            var response = await postService.CreatePostAsync(postRequest, cancellationToken);
            return response.IsSuccess ? Created() : response.ToProblem();
        }


        [HttpGet("{postId:int}")]
        public async Task<IActionResult> GetPost([FromRoute] int postId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching post with id: {id}", postId);
            var response = await postService.GetPostAsync(postId, cancellationToken);
            return response.IsSuccess ? Ok(response) : response.ToProblem();
        }
        [HttpPut]
        public async Task<IActionResult> UpdatePost([FromForm] UpdatePostRequest postRequest, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating post with id: {id}", postRequest.Id);
            var response = await postService.UpdatePostAsync(postRequest, cancellationToken);
            return response.IsSuccess ? Ok() : response.ToProblem();
        }
        [HttpDelete("{postId:int}")]
        public async Task<IActionResult> DeletePost([FromRoute] int postId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting post with id: {id}", postId);
            var response = await postService.DeletePostAsync(postId, cancellationToken);
            return response.IsSuccess ? NoContent() : response.ToProblem();
        }
    }
}
