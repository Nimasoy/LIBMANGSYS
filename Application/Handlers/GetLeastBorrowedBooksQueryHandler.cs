using Application.DTOs;
using Domain.Interfaces;
using Application.Queries;
using AutoMapper;
using MediatR;
using Application.Interfaces;

namespace Application.Handlers
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
