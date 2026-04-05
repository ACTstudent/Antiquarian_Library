namespace Antiquarian_Library.Models
{
    public class Book
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Year { get; set; }
        public int Total { get; set; }
        public int Available { get; set; }
        public decimal PenaltyPerDay { get; set; }
    }
}
