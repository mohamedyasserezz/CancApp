using FluentValidation;

namespace CancApp.Shared.Models.RecordAccess
{
    public class ApproveRecordAccessRequestValidator : AbstractValidator<ApproveRecordAccessRequest>
    {
        public ApproveRecordAccessRequestValidator()
        {
            RuleFor(x => x.RequestId)
                .GreaterThan(0)
                .WithMessage("enter a valid number greater than zero");
        }
    }
}
