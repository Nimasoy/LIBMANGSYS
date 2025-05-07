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

    }   
} 