using Application.Commands.Books;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers.Books
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, int>
    {
        private readonly IBookService _bookService;

        public AddBookCommandHandler(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<int> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            return await _bookService.AddBookAsync(request);
        }
    }
}
