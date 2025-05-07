using Domain.Interfaces;
using Application.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using Application.Interfaces;
using AutoMapper;
using Domain.Enums;
using Domain.Entities;

namespace Application.Handlers
{
    public class ReserveBookCommandHandler : IRequestHandler<ReserveBookCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReserveBookCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(ReserveBookCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var book = await _unitOfWork.Books.GetBookByIdAsync(request.BookId);
                var user = await _unitOfWork.Users.GetUserByIdAsync(request.UserId);

                if (book is null || user is null)
                    throw new Exception("Book or user not found.");

                if (book.Status == BookStatus.Available)
                    throw new Exception("Book is available — no need to reserve.");

                var activeBorrowCount = await _unitOfWork.Lendings.GetActiveBorrowedCountByUserAsync(user.Id);
                if (activeBorrowCount < 5)
                    throw new Exception("You are eligible to borrow — reservation not required.");

                var reservation = new Reservation
                {
                    BookId = book.Id,
                    UserId = user.Id,
                    Book = book,
                    User = user,
                    ReservedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(3)
                };

                await _unitOfWork.Reservations.AddReservationAsync(reservation);
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