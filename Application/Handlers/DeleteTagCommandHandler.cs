using Application.Commands;
using Application.Services;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers
{
    public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand, Unit>
    {
        private readonly TagService _tagService;

        public DeleteTagCommandHandler(TagService tagService)
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
