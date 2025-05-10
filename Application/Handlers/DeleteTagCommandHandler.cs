using Application.Commands;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers
{
    public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand, Unit>
    {
        private readonly ITagService _tagService;

        public DeleteTagCommandHandler(ITagService tagService)
        {
            _tagService = tagService;
        }

        public async Task<Unit> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            await _tagService.DeleteTagAsync(request.Id);
            return Unit.Value;
        }
    }


}
