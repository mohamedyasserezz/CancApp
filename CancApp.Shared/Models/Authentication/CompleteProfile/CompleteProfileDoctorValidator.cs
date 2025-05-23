﻿using FluentValidation;

namespace CancApp.Shared.Models.Authentication.CompleteProfile
{
    public class CompleteProfileDoctorValidator : AbstractValidator<CompleteProfileDoctor>
    {
        public CompleteProfileDoctorValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.");

            RuleFor(x => x.MedicalSyndicatePhoto)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}");

            RuleFor(x => x.ImageId)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}");
        }
    }
}
