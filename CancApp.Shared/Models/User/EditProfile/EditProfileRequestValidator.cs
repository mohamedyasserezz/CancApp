using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancApp.Shared.Models.User.EditProfile
{
    public class EditProfileRequestValidator : AbstractValidator<EditProfileRequest>
    {

        public EditProfileRequestValidator()
        {
            RuleFor(x => x.Name)
                .Length(3, 100)
                .WithMessage("{PropertyName} length should be between 3 and 100")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.Address)
                .Length(3, 100)
                .WithMessage("{PropertyName} length should be between 3 and 100")
                .When(x => !string.IsNullOrEmpty(x.Address));

        }
    }
}
