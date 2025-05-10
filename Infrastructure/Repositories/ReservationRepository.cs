using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly LibraryDbContext _context;
        public ReservationRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservation>> GetByUserIdAsync(int userId)
        {
            return await _context.Reservations.Where(r => r.UserId == userId).ToListAsync();
        }
        public async Task<int> GetActiveReservationCountByUserAsync(int userId)
        {
            return await _context.Reservations.CountAsync(r => r.UserId == userId && r.ExpiresAt > DateTime.UtcNow);

        }
        public async Task<Reservation?> GetActiveReservationAsync(int bookId, int userId)
        {
            return await _context.Reservations.FirstOrDefaultAsync(r => r.UserId == userId && r.BookId == bookId && r.ExpiresAt > DateTime.UtcNow);
        }
        public async Task<Reservation> AddReservationAsync(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
            return reservation;
        }

        public async Task<IEnumerable<Reservation>> GetActiveReservationsForBookAsync(int bookId)
        {
            return await _context.Reservations
                .Include(r => r.User)
                .Where(r => r.BookId == bookId && r.ExpiresAt > DateTime.UtcNow)
                .ToListAsync();
        }

        public Task RemoveReservationAsync(Reservation reservation)
        {
            _context.Reservations.Remove(reservation);
            return Task.CompletedTask;
        }

    }   
} 