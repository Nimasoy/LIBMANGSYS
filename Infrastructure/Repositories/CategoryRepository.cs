using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly LibraryDbContext _context;
        public CategoryRepository(LibraryDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {

            return await _context.Categories.ToListAsync();
        }
        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
        public async Task<IEnumerable<Book>> GetCategoryBooksAsync(int id)
        {
            return await _context.Books.Where(b => b.CategoryId == id).ToListAsync();
        }

        public async Task AddCategoryAsync(Category category) => await _context.Categories.AddAsync(category);

        public Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            return Task.CompletedTask;
        }

        public Task DeleteCategoryAsync(Category category)
        {
            _context.Categories.Remove(category);
            return Task.CompletedTask;
        }
    }
}