using Application.DTOs;
using MediatR;

namespace Application.Commands.Users
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }   
    }
}
