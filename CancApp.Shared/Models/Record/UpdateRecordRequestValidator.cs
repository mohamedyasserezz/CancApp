using FluentValidation;

namespace CancApp.Shared.Models.Record
{
    public class UpdateRecordRequestValidator : AbstractValidator<UpdateRecordRequest>
    {
        public UpdateRecordRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Plz Add a {PropertyName}");
        }
    }
}
