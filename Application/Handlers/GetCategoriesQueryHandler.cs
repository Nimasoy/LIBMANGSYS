using Application.DTOs;
using Domain.Interfaces;
using MediatR;
using AutoMapper;
using Application.Queries;
using Application.Services;

namespace Application.Handlers
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryDto>>
    {
        private readonly CategoryService _categoryService;

        public GetCategoriesQueryHandler(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IEnumerable<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _categoryService.GetCategoriesAsync();
        }
    }

}
