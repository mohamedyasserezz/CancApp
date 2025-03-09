using FluentValidation;

namespace CancApp.Shared.Models.Authentication.ResendConfirmationEmail
{
    public class ResendConfirmationEmailRequestValidator : AbstractValidator<ResendConfirmationEmailRequest>
    {
        public ResendConfirmationEmailRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
