using FluentValidation;

namespace CancApp.Shared.Models.Community.Post
{
    public class PostRequestValidator : AbstractValidator<PostRequest>
    {
        public PostRequestValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content is required.")
                .MaximumLength(1000)
                .WithMessage("Content must be less than 1000 characters.");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.");

        }

    }
}
