using FluentValidation;

namespace CancApp.Shared.Models.Community.Comments
{
    public class UpdateCommentRequestValidator : AbstractValidator<UpdateCommentRequest>
    {
        public UpdateCommentRequestValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content cannot be empty.")
                .MaximumLength(500)
                .WithMessage("Content cannot exceed 500 characters.");
        }
    }
}
