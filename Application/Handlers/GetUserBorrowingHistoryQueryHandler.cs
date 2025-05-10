using Application.DTOs;
using Application.Queries;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers
{
    public class GetUserBorrowingHistoryQueryHandler : IRequestHandler<GetUserBorrowingHistoryQuery, IEnumerable<BorrowingHistoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetUserBorrowingHistoryQueryHandler> _logger;

        public GetUserBorrowingHistoryQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<GetUserBorrowingHistoryQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<BorrowingHistoryDto>> Handle(
            GetUserBorrowingHistoryQuery request,
            CancellationToken cancellationToken)
        {
            var history = await _unitOfWork.Users.GetUserBorrowingHistoryAsync(request.UserId);
            return _mapper.Map<IEnumerable<BorrowingHistoryDto>>(history);
        }
    }
}
