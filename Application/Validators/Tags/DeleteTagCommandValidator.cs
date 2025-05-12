using Application.Commands.Tags;
using FluentValidation;

namespace Application.Validators.Tags
{
    public class DeleteTagCommandValidator : AbstractValidator<DeleteTagCommand>
    {
        public DeleteTagCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
