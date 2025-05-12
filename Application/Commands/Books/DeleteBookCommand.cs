using MediatR;

namespace Application.Commands.Books
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
