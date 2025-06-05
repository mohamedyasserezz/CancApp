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

        //public override async Task OnDisconnectedAsync(Exception? exception)
        //{
        //    var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (exception != null)
        //    {
        //        _logger.LogWarning(exception, "User {UserId} disconnected with ConnectionId {ConnectionId}", userId ?? "Anonymous", Context.ConnectionId);
        //    }
        //    else
        //    {
        //        _logger.LogInformation("User {UserId} disconnected with ConnectionId {ConnectionId}", userId ?? "Anonymous", Context.ConnectionId);
        //    }
        //    // SignalR automatically removes from groups on disconnect
        //    await base.OnDisconnectedAsync(exception);
        //}

        //public async Task JoinCommunity()
        //{
        //    try
        //    {
        //        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //        if (string.IsNullOrEmpty(userId))
        //        {
        //            _logger.LogWarning("Unauthenticated user attempted to join Community group");
        //            throw new HubException("User not authenticated");
        //        }
        //        await Groups.AddToGroupAsync(Context.ConnectionId, "Community");
        //        _logger.LogInformation("User {UserId} joined Community group with ConnectionId {ConnectionId}", userId, Context.ConnectionId);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error adding user to Community group");
        //        throw;
        //    }
        //}

        //public async Task LeaveCommunity()
        //{
        //    try
        //    {
        //        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Community");
        //        _logger.LogInformation("User {UserId} left Community group with ConnectionId {ConnectionId}", userId, Context.ConnectionId);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error removing user from Community group");
        //        throw;
        //    }
        //}

        // Methods the server will call on clients
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
