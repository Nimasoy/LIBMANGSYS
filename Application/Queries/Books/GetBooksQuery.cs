using MediatR;
using Application.DTOs;
namespace Application.Queries.Books
{
    public class GetBooksQuery : IRequest<IEnumerable<BookDto>> { }
}

