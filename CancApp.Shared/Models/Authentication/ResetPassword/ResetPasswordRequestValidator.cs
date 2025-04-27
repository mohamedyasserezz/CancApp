using CancApp.Shared.Common;
using FluentValidation;

namespace CancApp.Shared.Models.Authentication.ResetPassword
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Otp)
               .NotEmpty();

           
        }
    }
}
