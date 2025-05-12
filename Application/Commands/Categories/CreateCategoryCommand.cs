using MediatR;

namespace Application.Commands.Categories
{
    public class CreateCategoryCommand : IRequest<Unit>
    {
        public required string Name { get; set; }
    }
}
