using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BookRepository: IBookRepository
    {
        private readonly LibraryDbContext _context;
        
        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await _context.Books.AsTracking().Include(b => b.Category).Include(b => b.Tags).ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books.AsTracking().Include(b => b.Category).Include(b => b.Tags).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Book>> GetMostBorrowedAsync(int count)
        {
            return await _context.Books.Include(b => b.Category).Include(b => b.Tags).OrderByDescending(b => b.Lendings.Count).Take(count).ToListAsync();
        }
        public async Task<IEnumerable<Book>> GetLeastBorrowedAsync(int count)
        {
            return await _context.Books.Include(b => b.Category).Include(b => b.Tags).OrderByDescending(b => b.Lendings.Count).Take(count).ToListAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Book>> GetBorrowedBooksByUserId(int userId)
        {
            return await _context.Books.Where(b => b.Lendings.Any(l => l.UserId == userId)).ToListAsync();
        }
        public async Task AddBookAsync(Book book)
        {
            await _context.Books.AddAsync(book);
        }
        public Task DeleteBookAsync(Book book)
        {
            _context.Books.Remove(book);
            return Task.CompletedTask;
        }

    }
} 