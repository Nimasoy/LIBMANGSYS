using MediatR;

namespace Application.Commands.Users
{
    public class UpdateUserCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
    }
}
