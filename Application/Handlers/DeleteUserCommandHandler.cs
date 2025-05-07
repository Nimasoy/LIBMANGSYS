using Application.Commands;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Handlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(request.Id);
            if (user is null) throw new Exception("User not found.");
            await _unitOfWork.Users.DeleteUserAsync(user);

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }

}
