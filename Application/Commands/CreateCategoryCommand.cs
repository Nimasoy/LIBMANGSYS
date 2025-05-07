using MediatR;

namespace Application.Commands
{
    public class CreateCategoryCommand : IRequest<Unit>
    {
        public required string Name { get; set; }
    }
}
