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
    public class GetOverdueBooksQueryHandler : IRequestHandler<GetOverdueBooksQuery, IEnumerable<OverdueDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetOverdueBooksQueryHandler> _logger;

        public GetOverdueBooksQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<GetOverdueBooksQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<OverdueDto>> Handle(
            GetOverdueBooksQuery request,
            CancellationToken cancellationToken)
        {
            var overdueLendings = await _unitOfWork.Lendings.GetOverdueLendingsAsync();
            return _mapper.Map<IEnumerable<OverdueDto>>(overdueLendings);
        }
    }
}
