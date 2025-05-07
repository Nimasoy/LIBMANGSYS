using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public ICollection<Lending> BorrowedBooks { get; set; } = [];
        public ICollection<Reservation> Reservations { get; set; } = [];
        public int MaxBorrowLimit { get; set; } = 5;
    }
} 