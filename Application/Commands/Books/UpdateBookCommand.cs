using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Books
{
    public class UpdateBookCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public int CategoryId { get; set; }
        public required List<int> TagIds { get; set; }
    }

}
