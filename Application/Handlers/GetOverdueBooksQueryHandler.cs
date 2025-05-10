using Application.DTOs;
using Application.Interfaces;
using Application.Queries;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers
{
    public class GetOverdueBooksQueryHandler : IRequestHandler<GetOverdueBooksQuery, IEnumerable<OverdueDto>>
    {
        private readonly IBookService _bookService;

        public GetOverdueBooksQueryHandler(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<IEnumerable<OverdueDto>> Handle(GetOverdueBooksQuery request, CancellationToken cancellationToken)
        {
            return await _bookService.GetOverdueBooksAsync();
        }
    }

}
