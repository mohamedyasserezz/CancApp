using CancApp.Shared.Models.Community.Comments;
using CancApp.Shared.Models.Community.Post;
using CancApp.Shared.Models.Community.Reactions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace CanaApp.Application.Hups
{
    public class CommunityHub(ILogger<CommunityHub> logger) : Hub
    {
        private readonly ILogger<CommunityHub> _logger = logger;

        // Server methods clients can call
        public override async Task OnConnectedAsync()
        {
            try
            {
                var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, "Community");
                    _logger.LogInformation("User {UserId} auto-joined Community group with ConnectionId {ConnectionId}", userId, Context.ConnectionId);
                }
                else
                {
                    _logger.LogWarning("Unauthenticated user connected with ConnectionId {ConnectionId}", Context.ConnectionId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in OnConnectedAsync for ConnectionId {ConnectionId}", Context.ConnectionId);
                throw;
            }
            await base.OnConnectedAsync();
        }

   
        public async Task SendPostUpdate(PostResponse post)
        {
            await Clients.Group("Community").SendAsync("ReceivePostUpdate", post);
        }

        public async Task SendCommentUpdate(CommentResponse comment)
        {
            await Clients.Group("Community").SendAsync("ReceiveCommentUpdate", comment);
        }

        public async Task SendReactionUpdate(ReactionResponse reaction)
        {
            await Clients.Group("Community").SendAsync("ReceiveReactionUpdate", reaction);
        }

        public async Task SendPostDeleted(int postId)
        {
            await Clients.Group("Community").SendAsync("ReceivePostDeleted", postId);
        }

        public async Task SendCommentDeleted(int commentId)
        {
            await Clients.Group("Community").SendAsync("ReceiveCommentDeleted", commentId);
        }

        public async Task SendReactionRemoved(int reactionId)
        {
            await Clients.Group("Community").SendAsync("ReceiveReactionRemoved", reactionId);
        }
       
    }
}
