using MediatR;
using Application.Interfaces;
using Application.Commands.Login;

namespace Application.Handlers.Login
{
    public class CreateLoginCommandHandler : IRequestHandler<CreateLoginCommand, string>
    {
        private readonly IUserService _userService;

        public CreateLoginCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<string> Handle(CreateLoginCommand request, CancellationToken cancellationToken)
        {
            return await _userService.LoginAsync(request);
        }
    }

}
