using FluentValidation;

namespace CancApp.Shared.Models.Notification
{
    public class TokenRequestValidator : AbstractValidator<TokenRequest>
    {
        public TokenRequestValidator()
        {
   

            RuleFor(x => x.Token)
                .NotEmpty()
                .WithMessage("Token is required.")
                .Length(1, 1000)
                .WithMessage("Token must be between 1 and 1000 characters long.");
        }
    }
}
