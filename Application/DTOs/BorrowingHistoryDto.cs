namespace Application.DTOs
{
    public class BorrowingHistoryDto
    {
        public int Id { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string BookAuthor { get; set; } = string.Empty;
        public DateTime BorrowedAt { get; set; }
        public DateTime DueAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public bool IsOverdue => !ReturnedAt.HasValue && DateTime.UtcNow > DueAt;
    }
}
