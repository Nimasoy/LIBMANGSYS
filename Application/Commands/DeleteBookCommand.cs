using MediatR;

namespace Application.Commands
{
    public class DeleteBookCommand : IRequest<Unit>
    {
        public int Id { get; }
        public DeleteBookCommand(int id)
        {
            Id = id;
        }
    }

}
