namespace Domain.Entities
{
    public class Reservation 
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public required Book Book { get; set; }
        public int UserId { get; set; }
        public required User User { get; set; }
        public DateTime ReservedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
} 