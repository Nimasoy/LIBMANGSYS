using MediatR;

namespace Application.Commands
{
    public class CreateTagCommand : IRequest<Unit>
    {
        public required string Name { get; set; }
    }

}
