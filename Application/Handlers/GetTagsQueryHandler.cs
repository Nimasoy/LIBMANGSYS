using Application.Queries;
using Application.Services;
using Domain.Entities;
using MediatR;

namespace Application.Handlers
{
    public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, IEnumerable<Tag>>
    {
        private readonly TagService _tagService;

        public GetTagsQueryHandler(TagService tagService)
        {
            _tagService = tagService;
        }

        public async Task<IEnumerable<Tag>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            return await _tagService.GetTagsAsync();
        }
    }
}
