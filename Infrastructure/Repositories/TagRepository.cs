using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly LibraryDbContext _context;
        public TagRepository(LibraryDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Tag>> GetTagsAsync()
        {
            return await _context.Tags.ToListAsync();
        }
        public async Task<Tag?> GetTagByIdAsync(int id)
        {
            return await _context.Tags.FindAsync(id);
        }
        public async Task<IEnumerable<Book>> GetTagBooksAsync(int id)
        {
            return await _context.Books.Where(b => b.Tags.Any(t => t.Id == id)).ToListAsync();
        }

        public async Task AddTagAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
        }

        public async Task DeleteTagAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag != null)
                _context.Tags.Remove(tag);
        }

        public async Task<IEnumerable<Tag>> GetTagsByIdsAsync(IEnumerable<int> ids)
        {
            return await _context.Tags.Where(t => ids.Contains(t.Id)).ToListAsync();
        }


    }
} 