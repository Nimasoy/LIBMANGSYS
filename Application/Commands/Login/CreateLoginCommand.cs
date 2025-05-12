using MediatR;
namespace Application.Commands.Login
{ 
    public class CreateLoginCommand : IRequest<string>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }   
    }
}
