using CancApp.Shared.Models.Community.Comments;
using CancApp.Shared.Models.Community.Post;
using CancApp.Shared.Models.Community.Reactions;
using Microsoft.AspNetCore.SignalR;

namespace CanaApp.Application.Hups
{
    public class CommunityHub : Hub
    {
        // Server methods clients can call
        public async Task JoinCommunity()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Community");
        }

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
