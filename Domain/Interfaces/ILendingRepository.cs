using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ILendingRepository
    {
        Task<IEnumerable<Lending>> GetByUserIdAsync(int userId);
        Task<int> GetActiveBorrowedCountByUserAsync(int userId);
        Task<Lending?> GetActiveLendingAsync(int bookId, int userId);
        Task AddLendingAsync(Lending lending);
        Task UpdateLending(Lending lending);
        Task<IEnumerable<Lending>> GetOverdueLendingsAsync();
        Task<IEnumerable<Lending>> GetUserOverdueLendingsAsync(int userId);
    }
}