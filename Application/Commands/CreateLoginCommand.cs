using MediatR;
namespace Application.Commands
{ 
    public class CreateLoginCommand : IRequest<string>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }   
    }
}
