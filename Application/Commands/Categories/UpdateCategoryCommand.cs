using MediatR;

namespace Application.Commands.Categories
{
    public class UpdateCategoryCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }

}
