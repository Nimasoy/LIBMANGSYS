using Application.DTOs;
using Domain.Interfaces;
using MediatR;
using AutoMapper;
using Application.Interfaces;
using Application.Queries.Categories;

namespace Application.Handlers.Categories
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryDto>>
    {
        private readonly ICategoryService _categoryService;

        public GetCategoriesQueryHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IEnumerable<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _categoryService.GetCategoriesAsync();
        }
    }

}
