using Application.Commands;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, int>
    {
        private readonly BookService _bookService;

        public AddBookCommandHandler(BookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<int> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            return await _bookService.AddBookAsync(request);
        }
    }


}
