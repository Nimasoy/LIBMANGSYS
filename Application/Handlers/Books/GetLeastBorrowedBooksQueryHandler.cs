using Application.DTOs;
using Domain.Interfaces;
using AutoMapper;
using MediatR;
using Application.Interfaces;
using Application.Queries.Books;

namespace Application.Handlers.Books
{
    public class GetLeastBorrowedBooksQueryHandler : IRequestHandler<GetLeastBorrowedBooksQuery, IEnumerable<BookDto>>
    {
        private readonly IBookService _bookService;

        public GetLeastBorrowedBooksQueryHandler(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<IEnumerable<BookDto>> Handle(GetLeastBorrowedBooksQuery request, CancellationToken cancellationToken)
        {
            return await _bookService.GetLeastBorrowedAsync(request.Count);
        }
    }


}
