using Application.Commands;
using FluentValidation;

namespace Application.Validators
{
    public class CreateLoginCommandValidator : AbstractValidator<CreateLoginCommand>
    {
        public CreateLoginCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
