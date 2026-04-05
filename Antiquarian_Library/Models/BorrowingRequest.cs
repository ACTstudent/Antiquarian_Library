namespace Antiquarian_Library.Models
{
    public class BorrowingRequest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string BookTitle { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string StudentId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
    }
}
