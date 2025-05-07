using Application.Commands;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Categories.GetCategoryByIdAsync(request.Id);
            if (category == null) throw new Exception("Category not found.");
            category.Name = request.Name;
            await _unitOfWork.Categories.UpdateCategoryAsync(category);

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }

}
