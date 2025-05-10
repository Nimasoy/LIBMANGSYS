using Application.Commands;
using FluentValidation;

namespace Application.Validators
{
    public class DeleteTagCommandValidator : AbstractValidator<DeleteTagCommand>
    {
        public DeleteTagCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
