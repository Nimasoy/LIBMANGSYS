using Application.Commands.Login;
using FluentValidation;

namespace Application.Validators.Login
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
