using Application.Commands.Tags;
using FluentValidation;

namespace Application.Validators.Tags
{
    public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
    {
        public CreateTagCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
