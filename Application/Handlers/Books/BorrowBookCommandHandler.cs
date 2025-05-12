using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Domain.Enums;
using Domain.Entities;
using Application.Interfaces;
using Application.Commands.Books;

namespace Application.Handlers.Books
{
    public class BorrowBookCommandHandler : IRequestHandler<BorrowBookCommand, Unit>
    {
        private readonly IBookService _bookService;

        public BorrowBookCommandHandler(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<Unit> Handle(BorrowBookCommand request, CancellationToken cancellationToken)
        {
            await _bookService.BorrowBookAsync(request);
            return Unit.Value;
        }
    }

}