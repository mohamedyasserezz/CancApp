using FluentValidation;

namespace CancApp.Shared.Models.RecordAccess
{
    public class CreateRecordAccessRequestValidator : AbstractValidator<RecordAccessRequest>
    {
        public CreateRecordAccessRequestValidator()
        {
            RuleFor(x => x.DoctorId)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}");

            RuleFor(x => x.PatientId)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}")
                .NotEqual(x => x.DoctorId)
                .WithMessage("Doctor and patient must be different users.");
        }
    }
}
