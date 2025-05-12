using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Application.Queries.Tags;

namespace Application.Handlers.Tags
{
    public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, IEnumerable<Tag>>
    {
        private readonly ITagService _tagService;

        public GetTagsQueryHandler(ITagService tagService)
        {
            _tagService = tagService;
        }

        public async Task<IEnumerable<Tag>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            return await _tagService.GetTagsAsync();
        }
    }
}
