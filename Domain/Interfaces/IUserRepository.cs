using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRepository 
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<IEnumerable<Book>> GetBorrowedBooksByUserId(int id);

        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
    }
}