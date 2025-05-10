using Application.Commands;
using FluentValidation;

namespace Application.Validators
{
    public class DeleteBookCommandValidator : AbstractValidator<DeleteBookCommand>
    {
        public DeleteBookCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }

}
