using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }            
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User";

        public ICollection<Lending> BorrowedBooks { get; set; } = [];
        public ICollection<Reservation> Reservations { get; set; } = [];
        public int MaxBorrowLimit { get; set; } = 5;
    }
} 