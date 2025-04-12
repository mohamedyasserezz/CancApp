using FluentValidation;

namespace CancApp.Shared.Models.Community.Comments
{
    public class CommentRequestValidator : AbstractValidator<CommentRequest>
    {
        public CommentRequestValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content cannot be empty.")
                .MaximumLength(500)
                .WithMessage("Content cannot exceed 500 characters.");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId cannot be empty.");

            RuleFor(x => x.PostId)
                .NotEmpty()
                .WithMessage("PostId cannot be empty.");

        }
    }
   
}
