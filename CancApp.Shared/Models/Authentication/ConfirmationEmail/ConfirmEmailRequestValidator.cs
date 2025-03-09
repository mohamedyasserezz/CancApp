using FluentValidation;

namespace CancApp.Shared.Models.Authentication.ConfirmationEmail
{
    public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
    {
        public ConfirmEmailRequestValidator()
        {
            RuleFor(X => X.UserId)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}");

            RuleFor(X => X.Code)
               .NotEmpty()
               .WithMessage("Plz Add a {PropertyName}");
        }
    }
}
