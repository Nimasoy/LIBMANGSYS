using Application.DTOs;
using MediatR;

namespace Application.Queries.Books
{
    public class GetBookByIdQuery : IRequest<BookDto>
    {
        public int Id { get; set; }
        public GetBookByIdQuery(int id)
        {
            Id = id;
        }
        
    }
} 