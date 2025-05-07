using Domain.Interfaces;
using AutoMapper;
using Application.Commands;
using MediatR;
using Domain.Enums;

namespace Application.Handlers
{
    public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ReturnBookCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var book = await _unitOfWork.Books.GetBookByIdAsync(request.BookId);
                if (book == null || book.Status != BookStatus.Borrowed)
                    throw new Exception("Book is not currently borrowed.");

                var user = await _unitOfWork.Users.GetUserByIdAsync(request.UserId);
                if (user == null)
                    throw new Exception("User not found.");

                var lending = await _unitOfWork.Lendings.GetActiveLendingAsync(book.Id, user.Id);
                if (lending == null)
                    throw new Exception("No active lending record found for this book and user.");

                lending.ReturnedAt = DateTime.UtcNow;
                await _unitOfWork.Lendings.UpdateLending(lending);
                book.Status = BookStatus.Available;
                await _unitOfWork.Books.UpdateBook(book);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            return Unit.Value;
        }
    }
}