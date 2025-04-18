using FluentValidation;

namespace CancApp.Shared.Models.Authentication.CompleteProfile
{
    public class CompleteProfilePharmacyValidator : AbstractValidator<CompleteProfilePharmacy>
    {
        public CompleteProfilePharmacyValidator()
        {
            RuleFor(x => x.ImagePharmacyLicense)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}");

            RuleFor(x => x.ImageId)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}");

            RuleFor(x => x.NumberOfWorkingHours)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}")
                .Must(x => x > 0 && x <= 24)
                .WithMessage("{PropertyName} should be between 0 and 24");

            RuleFor(x => x.IsDeliveryEnabled)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}");



        }
    }
}
