using Application.DTOs;
using Domain.Interfaces;
using Application.Queries;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;

namespace Application.Handlers
{
    public class GetBorrowedBooksByUserIdQueryHandler : IRequestHandler<GetBorrowedBooksByUserIdQuery, IEnumerable<BookDto>>
    {
        private readonly IBookService _bookService;

        public GetBorrowedBooksByUserIdQueryHandler(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<IEnumerable<BookDto>> Handle(GetBorrowedBooksByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _bookService.GetBorrowedBooksByUserIdAsync(request.Id);
        }
    }


}
