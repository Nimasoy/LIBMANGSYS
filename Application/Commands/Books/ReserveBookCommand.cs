using System;
using MediatR;

namespace Application.Commands.Books
{
    public class ReserveBookCommand : IRequest<Unit>
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public ReserveBookCommand(int bookId, int userId)
        {
            BookId = bookId;
            UserId = userId;
        }
    }
}