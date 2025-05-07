using Application.Commands;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBookCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _unitOfWork.Books.GetBookByIdAsync(request.Id);
            if (book == null) throw new Exception("Book not found.");

            book.Title = request.Title;
            book.Author = request.Author;
            book.CategoryId = request.CategoryId;
            book.Tags = (await _unitOfWork.Tags.GetTagsByIdsAsync(request.TagIds)).ToList();

            await _unitOfWork.Books.UpdateBook(book);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }

}
