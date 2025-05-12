using Application.Commands.Categories;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers.Categories
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly ICategoryService _categoryService;

        public UpdateCategoryCommandHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            await _categoryService.UpdateCategoryAsync(request);
            return Unit.Value;
        }
    }


}
