using Application.Commands;
using FluentValidation;

namespace Application.Validators
{
    public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
    {
        public CreateTagCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
