using Application.DTOs;
using Application.Interfaces;
using Application.Queries;
using AutoMapper;
using MediatR;

namespace Application.Handlers
{
    public class GetLeastBorrowedBooksQueryHandler : IRequestHandler<GetLeastBorrowedBooksQuery, IEnumerable<BookDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetLeastBorrowedBooksQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookDto>> Handle(GetLeastBorrowedBooksQuery request, CancellationToken cancellationToken)
        {
            var books = await _unitOfWork.Books.GetLeastBorrowedAsync(request.Count);
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }
    }

}
