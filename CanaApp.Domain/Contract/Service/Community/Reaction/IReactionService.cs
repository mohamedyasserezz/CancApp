using CancApp.Shared.Abstractions;
using CancApp.Shared.Models.Community.Reactions;

namespace CanaApp.Domain.Contract.Service.Community.Reaction
{
    public interface IReactionService
    {
        Task<Result> AddReactionAsync(ReactionRequest request);
        Task<Result<IEnumerable<ReactionResponse>>> GetReactionsAsync(int postId, int? commentId);
        Task<Result> RemoveReactionAsync(ReactionRequest request);

    }
}
