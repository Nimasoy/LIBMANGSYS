using Application.DTOs;
using Domain.Interfaces;
using MediatR;
using AutoMapper;
using Application.Queries;
using Application.Interfaces;

namespace Application.Handlers
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
