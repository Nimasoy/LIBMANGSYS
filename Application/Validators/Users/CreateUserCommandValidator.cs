using Application.Commands.Users;
using FluentValidation;

namespace Application.Validators.Users
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Role)
            .Must(role => role == "User" || role == "Librarian")
            .WithMessage("Role must be 'User' or 'Librarian'");
        }
    }
}
