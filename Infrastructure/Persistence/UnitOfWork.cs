using Domain.Interfaces;
using Infrastructure.Data;
using Application.Interfaces;

namespace Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _context;

        public IBookRepository Books { get; }
        public ICategoryRepository Categories { get; }
        public ILendingRepository Lendings { get; }
        public IReservationRepository Reservations { get; }
        public ITagRepository Tags { get; }
        public IUserRepository Users { get; }

        public UnitOfWork(LibraryDbContext context, IBookRepository books, ICategoryRepository categories, ILendingRepository lendings,
            IReservationRepository reservations, ITagRepository tags, IUserRepository users)
        {
            _context = context;
            Books = books;
            Categories = categories;
            Lendings = lendings;
            Reservations = reservations;
            Tags = tags;
            Users = users;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }

}
