using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Polly.Registry;

namespace Application.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TagService> _logger;
        private readonly ICacheService _cache;
        private readonly ResiliencePipelineProvider<string> _pipelineProvider;

        public TagService(
            ITagRepository tagRepo,
            IUnitOfWork unitOfWork,
            ILogger<TagService> logger,
            ICacheService cache,
            ResiliencePipelineProvider<string> pipelineProvider)
        {
            _tagRepo = tagRepo;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cache = cache;
            _pipelineProvider = pipelineProvider;
        }

        public async Task<IEnumerable<Tag>> GetTagsAsync()
        {
            var pipeline = _pipelineProvider.GetPipeline("read-pipeline");
            return await pipeline.ExecuteAsync(async _ =>
            {
                const string key = "tag_list";
                var cached = await _cache.GetAsync<IEnumerable<Tag>>(key);
                if (cached is not null)
                {
                    _logger.LogInformation("Tags returned from cache");
                    return cached;
                }

                var tags = await _tagRepo.GetTagsAsync();
                await _cache.SetAsync(key, tags);
                return tags;
            });
        }

        public async Task CreateTagAsync(string name)
        {
            var tag = new Tag { Name = name };
            await _tagRepo.AddTagAsync(tag);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Tag created: {Name}", name);
            await _cache.RemoveAsync("tag_list");
        }

        public async Task DeleteTagAsync(int id)
        {
            await _tagRepo.DeleteTagAsync(id);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Tag deleted: Id={Id}", id);
            await _cache.RemoveAsync("tag_list");
        }
    }
}
