using Application.Commands;
using Application.DTOs;
using Application.Interfaces;
using Application.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class CategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;
        private readonly ICacheService _cache;


        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CategoryService> logger, ICacheService cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _cache = cache;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            const string key = "category_list";
            var cached = await _cache.GetAsync<IEnumerable<CategoryDto>>(key);
            if (cached is not null)
            {
                _logger.LogInformation("Categories returned from cache");
                return cached;
            }
            var categories = await _unitOfWork.Categories.GetCategoriesAsync();
            var dtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            await _cache.SetAsync(key, dtos);
            return dtos;
        }

        public async Task CreateCategoryAsync(CreateCategoryCommand request)
        {
            var category = new Category { Name = request.Name };
            await _unitOfWork.Categories.AddCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Category created: {Name}", request.Name);
            await _cache.RemoveAsync("category_list");
        }

        public async Task UpdateCategoryAsync(UpdateCategoryCommand request)
        {
            var category = await _unitOfWork.Categories.GetCategoryByIdAsync(request.Id);
            if (category == null)
            {
                _logger.LogWarning("Update failed: Category not found. Id={Id}", request.Id);
                throw new Exception("Category not found.");
            }
            category.Name = request.Name;
            await _unitOfWork.Categories.UpdateCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Category updated: Id={Id}", category.Id);
            await _cache.RemoveAsync("category_list");
        }

        public async Task DeleteCategoryAsync(DeleteCategoryCommand request)
        {
            var category = await _unitOfWork.Categories.GetCategoryByIdAsync(request.Id);
            if (category == null)
            {
                _logger.LogWarning("Delete failed: Category not found. Id={Id}", request.Id);
                throw new Exception("Category not found.");
            }
            await _unitOfWork.Categories.DeleteCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Category deleted: Id={Id}", category.Id);
            await _cache.RemoveAsync("category_list");
        }
    }
}
