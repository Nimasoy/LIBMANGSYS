using Application.Commands;
using Domain.Interfaces;
using AutoMapper;
using MediatR;
using Application.Services;

namespace Application.Handlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly UserService _userService;

        public DeleteUserCommandHandler(UserService userService)
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
