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

namespace Application.Handlers
{
    public class GetBorrowedBooksByUserIdQueryHandler : IRequestHandler<GetBorrowedBooksByUserIdQuery, IEnumerable<BookDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBorrowedBooksByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookDto>> Handle(GetBorrowedBooksByUserIdQuery request, CancellationToken cancellationToken)
        {
            var books = await _unitOfWork.Books.GetBorrowedBooksByUserId(request.Id);
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }
    }

}
