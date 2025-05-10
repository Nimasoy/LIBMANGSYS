using Application.Commands;
using FluentValidation;

namespace Application.Validators
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