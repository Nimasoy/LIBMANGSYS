using Application.DTOs;
using Application.Interfaces;
using Application.Queries;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers
{
    public class GetUserBorrowingHistoryQueryHandler : IRequestHandler<GetUserBorrowingHistoryQuery, IEnumerable<BorrowingHistoryDto>>
    {
        private readonly IUserService _userService;

        public GetUserBorrowingHistoryQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IEnumerable<BorrowingHistoryDto>> Handle(GetUserBorrowingHistoryQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetUserBorrowingHistoryAsync(request.UserId);
        }
    }
}
