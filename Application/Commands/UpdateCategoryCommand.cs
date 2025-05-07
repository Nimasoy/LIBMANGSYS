using MediatR;

namespace Application.Commands
{
    public class UpdateCategoryCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }

}
