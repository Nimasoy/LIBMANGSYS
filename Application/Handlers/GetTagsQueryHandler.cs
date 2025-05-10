using Application.Queries;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Handlers
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
