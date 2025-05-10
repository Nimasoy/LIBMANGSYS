using Application.Commands;
using Domain.Interfaces;
using AutoMapper;
using MediatR;
using Application.Interfaces;

namespace Application.Handlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IUserService _userService;

        public DeleteUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await _userService.DeleteUserAsync(request.Id);
            return Unit.Value;
        }
    }


}
