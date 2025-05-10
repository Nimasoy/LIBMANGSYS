using Application.Commands;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Unit>
    {
        private readonly ICategoryService _categoryService;

        public DeleteCategoryCommandHandler(ICategoryService categoryService)
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
