using MediatR;

namespace Application.Commands.Tags
{
    public class CreateTagCommand : IRequest<Unit>
    {
        public required string Name { get; set; }
    }

}
