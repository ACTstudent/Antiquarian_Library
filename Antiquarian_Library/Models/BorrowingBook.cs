namespace Antiquarian_Library.Models
{
    public class BorrowingBook
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string BookTitle { get; set; } = string.Empty;
        public string BorrowerName { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = "Active"; // Active, Returned RAH
    }
}
