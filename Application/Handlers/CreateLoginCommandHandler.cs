using Application.Commands;
using MediatR;

using Application.Services;

namespace Application.Handlers
{
    public class CreateLoginCommandHandler : IRequestHandler<CreateLoginCommand, string>
    {
        private readonly UserService _userService;

        public CreateLoginCommandHandler(UserService userService)
        {
            _userService = userService;
        }

        public async Task<string> Handle(CreateLoginCommand request, CancellationToken cancellationToken)
        {
            return await _userService.LoginAsync(request);
        }
    }

}
