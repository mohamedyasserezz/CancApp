using CancApp.Shared.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancApp.Shared.Models.Authentication.Register
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(X => X.Name)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}")
                .Length(3, 100)
                .WithMessage("{PropertyName} length should be between 3 and 100");

            RuleFor(X => X.UserName)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}")
                .Length(3, 100)
                .WithMessage("{PropertyName} length should be between 3 and 100");

            RuleFor(X => X.Address)
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


        }
    }
}
