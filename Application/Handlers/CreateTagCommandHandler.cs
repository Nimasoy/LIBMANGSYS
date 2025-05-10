using Application.Commands;
using Domain.Interfaces;
using Domain.Entities;
using MediatR;
using Application.Services;

namespace Application.Handlers
{
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, Unit>
    {
        private readonly TagService _tagService;

        public CreateTagCommandHandler(TagService tagService)
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
