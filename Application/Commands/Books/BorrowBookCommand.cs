using System;
using MediatR;

namespace Application.Commands.Books
{
    public class BorrowBookCommand : IRequest<Unit>
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public BorrowBookCommand(int bookId, int userId)
        {
            BookId = bookId;
            UserId = userId;
        }

    }
} 