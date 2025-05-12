using Application.DTOs;
using MediatR;

namespace Application.Queries.Books
{
    public class GetBorrowedBooksByUserIdQuery : IRequest<IEnumerable<BookDto>>
    {
        public int Id { get; set; }
        public GetBorrowedBooksByUserIdQuery(int id)
        {
            Id = id;
        }

    }
}