namespace Antiquarian_Library.Models
{
    public class EntryLog
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string StudentName { get; set; } = string.Empty;
        public string StudentId { get; set; } = string.Empty;
        public string YearLevel { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Time { get; set; } = string.Empty;
    }
}
