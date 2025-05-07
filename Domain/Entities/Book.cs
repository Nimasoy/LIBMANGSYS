using Domain.Enums;

namespace Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public int CategoryId { get; set; }
        public required Category Category { get; set; }
        public ICollection<Tag> Tags { get; set; } = [];
        public BookStatus Status { get; set; }
        public ICollection<Lending> Lendings { get; set; } = [];
        public ICollection<Reservation> Reservations { get; set; } = [];
    }
} 