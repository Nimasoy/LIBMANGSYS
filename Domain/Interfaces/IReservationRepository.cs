using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IReservationRepository 
    {
        Task<IEnumerable<Reservation>> GetByUserIdAsync(int userId);
        Task<int> GetActiveReservationCountByUserAsync(int userId);
        Task<Reservation?> GetActiveReservationAsync(int bookId, int userId);
        Task<Reservation> AddReservationAsync(Reservation reservation);
        Task<IEnumerable<Reservation>> GetActiveReservationsForBookAsync(int bookId);
        Task RemoveReservationAsync(Reservation reservation);
    }
}