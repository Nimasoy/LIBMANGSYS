using Application.DTOs;
using Application.Interfaces;
using MediatR;
using AutoMapper;
using Application.Queries;

namespace Application.Handlers
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetCategoriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _unitOfWork.Categories.GetCategoriesAsync();

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }
    }
}
