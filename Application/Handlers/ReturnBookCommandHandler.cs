using Domain.Interfaces;
using AutoMapper;
using Application.Commands;
using MediatR;
using Domain.Enums;
using Application.Interfaces;

namespace Application.Handlers
{
    public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand, Unit>
    {
        private readonly IBookService _bookService;

        public ReturnBookCommandHandler(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<Unit> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
        {
            await _bookService.ReturnBookAsync(request);
            return Unit.Value;
        }
    }

}