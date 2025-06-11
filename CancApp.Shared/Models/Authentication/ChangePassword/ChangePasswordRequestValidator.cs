using CancApp.Shared._Common.Consts;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancApp.Shared.Models.Authentication.ChangePassword
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty()
                .WithMessage("Old password is required.")
                .MinimumLength(6)
                .WithMessage("Old password must be at least 6 characters long.");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}")
                .Matches(RegexPatterns.Password)
                .WithMessage("Password should be atleast 8 digits and contains LowerCase, UpperCase, NonAlphanumeric");

            RuleFor(x => x)
            .Must(x => x.OldPassword != x.NewPassword)
            .WithMessage("New password must be different from the old password.");
        }
    }
}
