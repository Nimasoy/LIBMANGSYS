using Application.DTOs;
using Domain.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Queries.Books;

namespace Application.Handlers.Books
{
    public class GetMostBorrowedBooksQueryHandler : IRequestHandler<GetMostBorrowedBooksQuery, IEnumerable<BookDto>>
    {
        private readonly IBookService _bookService;

        public GetMostBorrowedBooksQueryHandler(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<IEnumerable<BookDto>> Handle(GetMostBorrowedBooksQuery request, CancellationToken cancellationToken)
        {
            return await _bookService.GetMostBorrowedAsync(request.Count);
        }
    }


}
