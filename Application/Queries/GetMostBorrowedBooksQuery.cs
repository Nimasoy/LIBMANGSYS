using Application.DTOs;
using MediatR;

namespace Application.Queries
{
    public class GetMostBorrowedBooksQuery : IRequest<IEnumerable<BookDto>>
    {
        public int Count { get; set; } = 5;
    }

}
