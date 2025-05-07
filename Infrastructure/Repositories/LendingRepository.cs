using Infrastructure.Data;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class LendingRepository : ILendingRepository
    {
        private readonly LibraryDbContext _context;
        public LendingRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Lending>> GetByUserIdAsync(int userId)
        {
            return await _context.Lendings.Where(l => l.UserId == userId).ToListAsync();
        }

        public async Task<int> GetActiveBorrowedCountByUserAsync(int userId)
        {
            return await _context.Lendings.CountAsync(l => l.UserId == userId && l.ReturnedAt == null);
        }

        public async Task<Lending?> GetActiveLendingAsync(int bookId, int userId)
        {
            return await _context.Lendings.FirstOrDefaultAsync(
               l =>
               l.BookId == bookId
            && l.UserId == userId
            && l.ReturnedAt == null);
        }
        public async Task AddLendingAsync(Lending lending)
        {
            await _context.Lendings.AddAsync(lending);
        }

        public Task UpdateLending(Lending lending)
        {
            _context.Lendings.Update(lending);
            return Task.CompletedTask;
        }
    }
}