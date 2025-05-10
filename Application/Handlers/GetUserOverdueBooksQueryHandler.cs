using Application.DTOs;
using Application.Queries;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class GetUserOverdueBooksQueryHandler : IRequestHandler<GetUserOverdueBooksQuery, IEnumerable<UserOverdueDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetUserOverdueBooksQueryHandler> _logger;

        public GetUserOverdueBooksQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<GetUserOverdueBooksQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<UserOverdueDto>> Handle(
            GetUserOverdueBooksQuery request,
            CancellationToken cancellationToken)
        {
            var overdueLendings = await _unitOfWork.Lendings.GetUserOverdueLendingsAsync(request.UserId);
            return _mapper.Map<IEnumerable<UserOverdueDto>>(overdueLendings);
        }
    }
}
