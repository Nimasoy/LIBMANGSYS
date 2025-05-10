using Application.Commands;
using Application.Services;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Unit>
    {
        private readonly CategoryService _categoryService;

        public DeleteCategoryCommandHandler(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            await _categoryService.DeleteCategoryAsync(request);
            return Unit.Value;
        }
    }


}
