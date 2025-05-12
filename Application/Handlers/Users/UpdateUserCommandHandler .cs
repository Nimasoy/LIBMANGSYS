using Domain.Interfaces;
using AutoMapper;
using MediatR;
using Application.Interfaces;
using Application.Commands.Users;

namespace Application.Handlers.Users
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IUserService _userService;

        public UpdateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            await _userService.UpdateUserAsync(request);
            return Unit.Value;
        }
    }


}
