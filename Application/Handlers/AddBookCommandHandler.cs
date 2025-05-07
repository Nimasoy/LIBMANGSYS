using Application.Commands;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddBookCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Categories.GetCategoryByIdAsync(request.CategoryId);
            if (category is null) throw new Exception("Category not found.");

            var tags = (await _unitOfWork.Tags.GetTagsByIdsAsync(request.TagIds)).ToList();
            var book = new Book
            {
                Title = request.Title,
                Author = request.Author,
                CategoryId = request.CategoryId,
                Category = category,
                Tags = (await _unitOfWork.Tags.GetTagsByIdsAsync(request.TagIds)).ToList(),
                Status = BookStatus.Available
            };

            await _unitOfWork.Books.AddBookAsync(book);
            await _unitOfWork.SaveChangesAsync();
            return book.Id;
        }
    }

}
