using Application.Commands;
using Domain.Interfaces;
using AutoMapper;
using MediatR;
using Application.Services;

namespace Application.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly UserService _userService;

        public UpdateUserCommandHandler(UserService userService)
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
