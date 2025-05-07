using Application.Commands;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(request.Id);
            if (user is null) throw new Exception("User not found.");
            user.UserName = request.UserName;
            user.Email = request.Email;
            await _unitOfWork.Users.UpdateUserAsync(user);

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }

}
