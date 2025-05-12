using Application.Commands.Books;
using FluentValidation;

namespace Application.Validators.Books
{
    public class DeleteBookCommandValidator : AbstractValidator<DeleteBookCommand>
    {
        public DeleteBookCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }

}
