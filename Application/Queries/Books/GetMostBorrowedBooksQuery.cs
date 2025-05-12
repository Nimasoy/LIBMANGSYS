using Application.DTOs;
using MediatR;

namespace Application.Queries.Books
{
    public class GetMostBorrowedBooksQuery : IRequest<IEnumerable<BookDto>>
    {
        public int Count { get; set; } = 5;
    }

}
