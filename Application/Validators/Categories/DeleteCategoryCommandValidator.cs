using Application.Commands.Categories;
using FluentValidation;

namespace Application.Validators.Categories
{
    public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
