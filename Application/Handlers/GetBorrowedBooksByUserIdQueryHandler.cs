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
using Application.Services;

namespace Application.Handlers
{
    public class GetBorrowedBooksByUserIdQueryHandler : IRequestHandler<GetBorrowedBooksByUserIdQuery, IEnumerable<BookDto>>
    {
        private readonly BookService _bookService;

        public GetBorrowedBooksByUserIdQueryHandler(BookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<IEnumerable<BookDto>> Handle(GetBorrowedBooksByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _bookService.GetBorrowedBooksByUserIdAsync(request.Id);
        }
    }


}
