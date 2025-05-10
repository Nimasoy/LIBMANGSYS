using Domain.Interfaces;
using Application.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Domain.Enums;
using Domain.Entities;
using Application.Interfaces;

namespace Application.Handlers
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