using Application.DTOs;
using Application.Interfaces;
using MediatR;
using AutoMapper;
using Application.Queries;

namespace Application.Handlers
{
    public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, IEnumerable<BookDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetBooksQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            var books = await _unitOfWork.Books.GetBooksAsync();

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<IEnumerable < BookDto >> (books);
        }
    }
}
