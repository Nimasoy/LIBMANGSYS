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
            return await _context.Books.ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<IEnumerable<Book>> GetMostBorrowedAsync(int count)
        {
            return await _context.Books.OrderByDescending(b => b.Lendings.Count).Take(count).ToListAsync();
        }
        public async Task<IEnumerable<Book>> GetLeastBorrowedAsync(int count)
        {
            return await _context.Books.OrderBy(b => b.Lendings.Count).Take(count).ToListAsync();
        }

        public Task UpdateBook(Book book)
        {
            _context.Books.Update(book);
            return Task.CompletedTask;
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