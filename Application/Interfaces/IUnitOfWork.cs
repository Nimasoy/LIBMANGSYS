using Domain.Interfaces;

namespace Application.Interfaces
{
    public interface IUnitOfWork
    {
        IBookRepository Books { get; }
        ICategoryRepository Categories { get; }
        ILendingRepository Lendings { get; }
        IReservationRepository Reservations { get; }
        ITagRepository Tags { get; }
        IUserRepository Users { get; }
        Task<int> SaveChangesAsync();
    }
}
