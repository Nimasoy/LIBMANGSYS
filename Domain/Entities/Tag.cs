namespace Domain.Entities
{
    public class Tag 
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
} 