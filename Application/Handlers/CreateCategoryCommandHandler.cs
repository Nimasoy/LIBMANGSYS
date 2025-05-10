using Application.Commands;
using Domain.Interfaces;
using Domain.Entities;
using MediatR;
using Application.Services;

namespace Application.Handlers
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Unit>
    {
        private readonly CategoryService _categoryService;

        public CreateCategoryCommandHandler(CategoryService categoryService)
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
