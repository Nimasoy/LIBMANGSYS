using Application.Commands;
using Application.Services;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly CategoryService _categoryService;

        public UpdateCategoryCommandHandler(CategoryService categoryService)
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
