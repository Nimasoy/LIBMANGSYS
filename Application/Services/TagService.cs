using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class TagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TagService> _logger;
        private readonly ICacheService _cache;

        public TagService(IUnitOfWork unitOfWork, ILogger<TagService> logger, ICacheService cache)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cache = cache;
        }

        public async Task<IEnumerable<Tag>> GetTagsAsync()
        {
            const string key = "tag_list";
            var cached = await _cache.GetAsync<IEnumerable<Tag>>(key);
            if (cached is not null)
            {
                _logger.LogInformation("Tags returned from cache");
                return cached;
            }

            var tags = await _unitOfWork.Tags.GetTagsAsync();
            await _cache.SetAsync(key, tags);
            return tags;
        }

        public async Task CreateTagAsync(string name)
        {
            var tag = new Tag { Name = name };
            await _unitOfWork.Tags.AddTagAsync(tag);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Tag created: {Name}", name);
            await _cache.RemoveAsync("tag_list");
        }

        public async Task DeleteTagAsync(int id)
        {
            await _unitOfWork.Tags.DeleteTagAsync(id);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Tag deleted: Id={Id}", id);
            await _cache.RemoveAsync("tag_list");
        }
    }
}
