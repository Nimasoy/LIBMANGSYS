using Domain.Interfaces;
using Domain.Entities;
using MediatR;
using Application.Interfaces;
using Application.Commands.Categories;

namespace Application.Handlers.Categories
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Unit>
    {
        private readonly ICategoryService _categoryService;

        public CreateCategoryCommandHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<Unit> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            await _categoryService.CreateCategoryAsync(request);
            return Unit.Value;
        }
    }


}
