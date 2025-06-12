using CancApp.Shared._Common.Consts;
using FluentValidation;

namespace CancApp.Shared.Models.Record
{
    public class RecordRequestValidator : AbstractValidator<RecordRequest>
    {
        public RecordRequestValidator()
        {
            RuleFor(x => x.Notes)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}");

           RuleFor(x => x.Date)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}");

            RuleFor(x => x.RecordType)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}")
                .Must(x => x is Records.Scan or Records.LabResult or Records.Document or Records.Prescription)
                .WithMessage("Record must be Scan or Document or LabResult or Prescription");
        }
    }
}
