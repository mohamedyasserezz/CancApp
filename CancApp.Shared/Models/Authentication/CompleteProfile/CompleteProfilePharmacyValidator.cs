using FluentValidation;

namespace CancApp.Shared.Models.Authentication.CompleteProfile
{
    public class CompleteProfilePharmacyValidator : AbstractValidator<CompleteProfilePharmacyRequest>
    {
        public CompleteProfilePharmacyValidator()
        {
            RuleFor(x => x.ImagePharmacyLicense)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}");

            RuleFor(x => x.ImageId)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}");

            RuleFor(x => x.OpenHour)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}")
                .Must(x => BeValidHour(x))
                .WithMessage("{PropertyName} should be between 0 and 24");

            RuleFor(x => x.ClouseHour)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}")
                .Must(x => BeValidHour(x))
                .WithMessage("{PropertyName} should be between 0 and 24");

            RuleFor(x => x.IsDeliveryEnabled)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}");

            RuleFor(x => x.Latitude)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}")
                .InclusiveBetween(-90, 90)
                .WithMessage("{PropertyName} must be between -90 and 90");

            RuleFor(x => x.Longitude)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}")
                .InclusiveBetween(-180, 180)
                .WithMessage("{PropertyName} must be between -180 and 180");

        }
        private bool BeValidHour(TimeOnly time)
        {
            // Hours between 00 and 23, minutes between 00 and 59
            return time.Hour >= 0 && time.Hour <= 23 && time.Minute >= 0 && time.Minute <= 59;
        }
    }
}
