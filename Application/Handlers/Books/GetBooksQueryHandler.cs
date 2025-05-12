using Application.DTOs;
using Domain.Interfaces;
using MediatR;
using AutoMapper;
using Application.Interfaces;
using Application.Queries.Books;

namespace Application.Handlers.Books
{
    public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, IEnumerable<BookDto>>
    {
        private readonly IBookService _bookService;

        public GetBooksQueryHandler(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<IEnumerable<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            return await _bookService.GetBooksAsync();
        }
    }

}
