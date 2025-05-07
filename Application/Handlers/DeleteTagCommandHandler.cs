using Application.Commands;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers
{
    public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTagCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Tags.DeleteTagAsync(request.Id);

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }

}
