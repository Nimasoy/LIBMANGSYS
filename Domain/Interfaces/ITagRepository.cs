using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetTagsAsync();  
        Task<Tag?> GetTagByIdAsync(int id);
        Task<IEnumerable<Book>> GetTagBooksAsync(int id);

        Task AddTagAsync(Tag tag);
        Task DeleteTagAsync(int id);

    }
}