using Domain.Interfaces;
using Application.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Domain.Enums;
using Domain.Entities;

namespace Application.Handlers
{
    public class BorrowBookCommandHandler : IRequestHandler<BorrowBookCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BorrowBookCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(BorrowBookCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var book = await _unitOfWork.Books.GetBookByIdAsync(request.BookId);
                if (book == null || book.Status != BookStatus.Available)
                    throw new Exception("Book is not available for borrowing.");

                var user = await _unitOfWork.Users.GetUserByIdAsync(request.UserId);
                if (user == null)
                    throw new Exception("User not found.");

                var userBorrowedCount = await _unitOfWork.Lendings.GetActiveBorrowedCountByUserAsync(user.Id);
                if (userBorrowedCount >= user.MaxBorrowLimit)
                    throw new Exception("User has reached the maximum borrow limit.");

                var lending = new Lending
                {
                    BookId = book.Id,
                    UserId = user.Id,
                    Book = book,
                    User = user,
                    BorrowedAt = DateTime.UtcNow,
                    DueAt = DateTime.UtcNow.AddDays(14)
                };
                await _unitOfWork.Lendings.AddLendingAsync(lending);
                book.Status = BookStatus.Borrowed;
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