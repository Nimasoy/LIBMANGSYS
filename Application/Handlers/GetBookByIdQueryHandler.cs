using Application.DTOs;
using Application.Queries;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Handlers
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetBookByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BookDto?> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var book = await _unitOfWork.Books.GetBookByIdAsync(request.Id);
            if (book == null) return null;
            var dto = _mapper.Map<BookDto>(book);

            await _unitOfWork.SaveChangesAsync();

            return dto;
        }
    }
} 