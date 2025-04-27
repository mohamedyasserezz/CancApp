
using CancApp.Shared.Common;
using FluentValidation;

namespace CancApp.Shared.Models.Authentication.ResetPassword
{
    public class AssignNewPasswordValidator : AbstractValidator<AssignNewPassword>
    {
        public AssignNewPasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.NewPassword)
               .NotEmpty()
               .Matches(RegexPatterns.Password)
               .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");
        }


    }

}
