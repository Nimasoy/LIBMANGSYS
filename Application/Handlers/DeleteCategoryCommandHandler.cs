using Application.Commands;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Categories.GetCategoryByIdAsync(request.Id);
            if (category == null) throw new Exception("Category not found.");
            await _unitOfWork.Categories.DeleteCategoryAsync(category);

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }

}
