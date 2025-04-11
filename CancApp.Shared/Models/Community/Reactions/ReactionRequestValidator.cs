using CancApp.Shared.Models.Authentication.ConfirmationEmail;
using FluentValidation;

namespace CancApp.Shared.Models.Community.Reactions
{
    class ReactionRequestValidator : AbstractValidator<ReactionRequest>
    {
        private static readonly IReadOnlyCollection<string> ValidReactionTypes = new[]
        {
            "Like",
            "Love",
            "Haha",
            "Wow",
            "Sad",
            "Angry"
        };
        public ReactionRequestValidator()
        {
            RuleFor(X => X.UserId)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}");

            RuleFor(X => X.PostId)
               .NotEmpty()
               .WithMessage("Plz Add a {PropertyName}");

            RuleFor(X => X.ReactionType)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}")
                .Must(BeValidReactionType).WithMessage($"Reaction type must be one of: {string.Join(", ", ValidReactionTypes)}");

            RuleFor(X => X.IsComment)
                .NotNull()
                .WithMessage("Identify if the reaction for post or comment");

            RuleFor(x => x.CommentId)
                .NotEmpty()
                .When(x => x.IsComment)
                .WithMessage("Plz Add a {PropertyName}");  

        }

        private bool BeValidReactionType(string reactionType)
        {
            return ValidReactionTypes.Contains(reactionType, StringComparer.OrdinalIgnoreCase);
        }
    }
}
