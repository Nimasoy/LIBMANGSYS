using Application.DTOs;
using MediatR;

namespace Application.Queries.Users
{
    public class GetUserBorrowingHistoryQuery : IRequest<IEnumerable<BorrowingHistoryDto>>
    {
        public int UserId { get; set; }
        public GetUserBorrowingHistoryQuery(int userId)
        {
            UserId = userId;
        }
    }
}
