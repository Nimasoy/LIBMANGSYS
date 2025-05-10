using Application.DTOs;
using Application.Queries;
using Domain.Interfaces;
using AutoMapper;
using MediatR;
using Application.Services;

namespace Application.Handlers
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookDto>
    {
        private readonly BookService _bookService;

        public GetBookByIdQueryHandler(BookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<BookDto> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            return await _bookService.GetBookByIdAsync(request.Id);
        }
    }

}