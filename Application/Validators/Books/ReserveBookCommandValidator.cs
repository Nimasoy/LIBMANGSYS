using Application.Commands.Books;
using FluentValidation;

namespace Application.Validators.Books
{
    public class ReserveBookCommandValidator : AbstractValidator<ReserveBookCommand>
    {
        public ReserveBookCommandValidator()
        {
            RuleFor(x => x.BookId).GreaterThan(0);
            RuleFor(x => x.UserId).GreaterThan(0);
        }
    }
}