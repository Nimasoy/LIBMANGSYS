using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Books
{
    public class GetUserOverdueBooksQuery : IRequest<IEnumerable<UserOverdueDto>>
    {
        public int UserId { get; set; }
        public GetUserOverdueBooksQuery(int userId)
        {
            UserId = userId;
        }
    }
}
