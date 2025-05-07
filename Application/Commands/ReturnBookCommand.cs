using System;
using MediatR;

namespace Application.Commands
{
    public class ReturnBookCommand : IRequest<Unit>
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
    }
} 