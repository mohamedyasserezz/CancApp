using CancApp.Shared.Common.Consts;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancApp.Shared.Models.User.EditProfile
{
    public class GetAllUsersRequestValidator : AbstractValidator<GetAllUsersRequest>
    {
        public GetAllUsersRequestValidator()
        {
            RuleFor(request => request.UserType)
                .NotEmpty()
                .WithMessage("User type is required.")
                .Must(x => x is Users.Patient or Users.Doctor or Users.Volunteer or Users.Psychiatrist or Users.Pharmacist)
                .WithMessage("Invalid user type specified.");
        }


    }
}
