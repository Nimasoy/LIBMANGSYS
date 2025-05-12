using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Books
{
    public class GetLeastBorrowedBooksQuery : IRequest<IEnumerable<BookDto>>
    {
        public int Count { get; set; } = 5;
    }

}
