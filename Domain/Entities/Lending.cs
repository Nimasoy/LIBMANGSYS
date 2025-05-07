namespace Domain.Entities
{
    public class Lending
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public required Book Book { get; set; }
        public int UserId { get; set; }
        public required User User { get; set; }
        public DateTime BorrowedAt { get; set; }
        public DateTime DueAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
    }
} 