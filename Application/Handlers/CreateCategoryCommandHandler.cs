using Application.Commands;
using Domain.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Handlers
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category { Name = request.Name };
            await _unitOfWork.Categories.AddCategoryAsync(category);

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }

}
