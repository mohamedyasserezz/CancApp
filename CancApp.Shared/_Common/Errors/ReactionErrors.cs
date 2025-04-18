using CancApp.Shared.Abstractions;
using Microsoft.AspNetCore.Http;

namespace CancApp.Shared.Common.Errors
{
    public static class ReactionErrors
    {
        public static readonly Error ReactionNotFound =
            new("Reaction.ReactionNotFound", "Reaction is not found", StatusCodes.Status404NotFound);
        public static readonly Error ReactionAlreadyExists =
            new("Reaction.ReactionAlreadyExists", "Reaction already exists", StatusCodes.Status409Conflict);
        public static readonly Error InvalidReactionType =
            new("Reaction.InvalidReactionType", "Invalid reaction type", StatusCodes.Status400BadRequest);
    }
}
