using Application.Commands.Categories;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Polly.Registry;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;
        private readonly ICacheService _cache;
        private readonly ResiliencePipelineProvider<string> _pipelineProvider;

        public CategoryService(
            ICategoryRepository categoryRepo,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<CategoryService> logger,
            ICacheService cache, ResiliencePipelineProvider<string> pipelineProvider)
        {
            _categoryRepo = categoryRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _cache = cache;
            _pipelineProvider = pipelineProvider;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            var pipeline = _pipelineProvider.GetPipeline("read-pipeline");
            return await pipeline.ExecuteAsync(async _ =>
            {
                const string key = "category_list";
                var cached = await _cache.GetAsync<IEnumerable<CategoryDto>>(key);
                if (cached is not null)
                {
                    _logger.LogInformation("Categories returned from cache");
                    return cached;
                }

                var categories = await _categoryRepo.GetCategoriesAsync();
                var dtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
                await _cache.SetAsync(key, dtos);
                return dtos;
            });
        }

        public async Task CreateCategoryAsync(CreateCategoryCommand request)
        {
            var category = new Category { Name = request.Name };
            await _categoryRepo.AddCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Category created: {Name}", request.Name);
            await _cache.RemoveAsync("category_list");
        }

        public async Task UpdateCategoryAsync(UpdateCategoryCommand request)
        {
            var category = await _categoryRepo.GetCategoryByIdAsync(request.Id);
            if (category == null)
            {
                _logger.LogWarning("Update failed: Category not found. Id={Id}", request.Id);
                throw new Exception("Category not found.");
            }

            category.Name = request.Name;
            await _categoryRepo.UpdateCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Category updated: Id={Id}", category.Id);
            await _cache.RemoveAsync("category_list");
        }

        public async Task DeleteCategoryAsync(DeleteCategoryCommand request)
        {
            var category = await _categoryRepo.GetCategoryByIdAsync(request.Id);
            if (category == null)
            {
                _logger.LogWarning("Delete failed: Category not found. Id={Id}", request.Id);
                throw new Exception("Category not found.");
            }

            await _categoryRepo.DeleteCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Category deleted: Id={Id}", category.Id);
            await _cache.RemoveAsync("category_list");
        }
    }
}
