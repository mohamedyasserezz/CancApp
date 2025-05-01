using CancApp.Shared.Common.Consts;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CancApp.Shared._Common.Consts;

namespace CancApp.Shared.Models.Authentication.Register
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(X => X.FullName)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}")
                .Length(3, 100)
                .WithMessage("{PropertyName} length should be between 3 and 100");

            RuleFor(X => X.FullName)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}")
                .Length(3, 100)
                .WithMessage("{PropertyName} length should be between 3 and 100");

            RuleFor(X => X.Address)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}")
               .Length(3, 100)
               .WithMessage("{PropertyName} length should be between 3 and 100")
               .When(X => !string.IsNullOrEmpty(X.Address));

            RuleFor(X => X.Email)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}")
                .EmailAddress();


            RuleFor(X => X.Password)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}")
                .Matches(RegexPatterns.Password)
                .WithMessage("Password should be atleast 8 digits and contains LowerCase, UpperCase, NonAlphanumeric");
            RuleFor(X => X.UserType)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}")
                .Must(x => x == Users.Admin || x == Users.Patient
                || x == Users.Volunteer || x == Users.Pharmacist || x == Users.Doctor || x == Users.Psychiatrist)
                .WithMessage("UserType should be either User or Admin");

        }
    }
}
