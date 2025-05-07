using MediatR;
using Application.DTOs;
namespace Application.Queries
{
    public class GetBooksQuery : IRequest<IEnumerable<BookDto>> { }
}

