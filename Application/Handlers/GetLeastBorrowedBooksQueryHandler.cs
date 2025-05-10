using Application.DTOs;
using Domain.Interfaces;
using Application.Queries;
using AutoMapper;
using MediatR;
using Application.Services;

namespace Application.Handlers
{
    public class GetLeastBorrowedBooksQueryHandler : IRequestHandler<GetLeastBorrowedBooksQuery, IEnumerable<BookDto>>
    {
        private readonly BookService _bookService;

        public GetLeastBorrowedBooksQueryHandler(BookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<IEnumerable<BookDto>> Handle(GetLeastBorrowedBooksQuery request, CancellationToken cancellationToken)
        {
            return await _bookService.GetLeastBorrowedAsync(request.Count);
        }
    }


}
