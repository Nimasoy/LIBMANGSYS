using Domain.Interfaces;
using Domain.Entities;
using MediatR;
using Application.Interfaces;
using Application.Commands.Tags;

namespace Application.Handlers.Tags
{
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, Unit>
    {
        private readonly ITagService _tagService;

        public CreateTagCommandHandler(ITagService tagService)
        {
            _tagService = tagService;
        }

        public async Task<Unit> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            await _tagService.CreateTagAsync(request.Name);
            return Unit.Value;
        }
    }


}
