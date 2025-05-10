using Application.Commands;
using Application.Services;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Unit>
    {
        private readonly BookService _bookService;

        public DeleteBookCommandHandler(BookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<Unit> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            await _bookService.DeleteBookAsync(request.Id);
            return Unit.Value;
        }
    }


}
