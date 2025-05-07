using Application.Commands;
using Domain.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Handlers
{
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateTagCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        } 

        public async Task<Unit> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = new Tag { Name = request.Name };
            await _unitOfWork.Tags.AddTagAsync(tag);

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }

}
