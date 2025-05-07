using Domain.Interfaces;

namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IBookRepository Books { get; }
        ICategoryRepository Categories { get; }
        ILendingRepository Lendings { get; }
        IReservationRepository Reservations { get; }
        ITagRepository Tags { get; }
        IUserRepository Users { get; }

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<int> SaveChangesAsync();
    }
}
