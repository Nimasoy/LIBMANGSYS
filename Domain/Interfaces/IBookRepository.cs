using Domain.Entities;
namespace Domain.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
        Task UpdateBook(Book book);
        Task<IEnumerable<Book>> GetBorrowedBooksByUserId(int userId);

        Task<IEnumerable<Book>> GetMostBorrowedAsync(int count);
        Task<IEnumerable<Book>> GetLeastBorrowedAsync(int count);
        Task AddBookAsync(Book book);
        Task DeleteBookAsync(Book book);

    }
}