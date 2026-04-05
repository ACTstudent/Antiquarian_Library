using Antiquarian_Library.Models;
using System.Security.Cryptography;

namespace Antiquarian_Library.Services
{
    public class LocalDatabaseService
    {
        private static List<Book> _books = new List<Book>
        {
            new Book { Id = "1", Title = "Pride and Prejudice", Author = "Jane Austen", ISBN = "978-0141439518", Category = "Classic Literature", Year = 1813, Total = 5, Available = 3, PenaltyPerDay = 5.0m },
            new Book { Id = "2", Title = "Moby-Dick", Author = "Herman Melville", ISBN = "978-0142437247", Category = "Adventure", Year = 1851, Total = 4, Available = 2, PenaltyPerDay = 7.50m },
            new Book { Id = "3", Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", ISBN = "978-0743273565", Category = "Fiction", Year = 1925, Total = 6, Available = 4, PenaltyPerDay = 10.00m }
        };

        // Create a default admin user. Actual password hashing handles on Create
        private static List<User> _users = new List<User>
        {
            new User { Id = "1", Username = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), Role = "Administrator" }
        };

        private static List<BorrowingRequest> _requests = new List<BorrowingRequest>
        {
            new BorrowingRequest { Id = "1", BookTitle = "Pride and Prejudice", StudentName = "Emily Watson", StudentId = "STU001", Email = "emily.watson@university.edu", Phone = "555-0123", RequestDate = DateTime.Parse("2026-03-20"), ReturnDate = DateTime.Parse("2026-04-03"), Status = "Pending" }
        };

        private static List<EntryLog> _logs = new List<EntryLog>
        {
            new EntryLog { Id = "1", StudentName = "John Smith", StudentId = "STU002", YearLevel = "3rd Year", Date = DateTime.Parse("2026-03-23"), Time = "09:30 AM" }
        };

        private static List<BorrowingBook> _borrowed = new List<BorrowingBook>();

        // --- Books ---
        public Task<List<Book>> GetBooksAsync() => Task.FromResult(_books.ToList());

        public Task<Book?> GetBookAsync(string id) => Task.FromResult(_books.FirstOrDefault(b => b.Id == id));

        public Task CreateBookAsync(Book book)
        {
            book.Id = Guid.NewGuid().ToString();
            _books.Add(book);
            return Task.CompletedTask;
        }

        public Task UpdateBookAsync(string id, Book book)
        {
            var existing = _books.FirstOrDefault(b => b.Id == id);
            if (existing != null)
            {
                existing.Title = book.Title;
                existing.Author = book.Author;
                existing.ISBN = book.ISBN;
                existing.Category = book.Category;
                existing.Year = book.Year;
                existing.Total = book.Total;
                existing.Available = book.Available;
                existing.PenaltyPerDay = book.PenaltyPerDay;
            }
            return Task.CompletedTask;
        }

        public Task DeleteBookAsync(string id)
        {
            _books.RemoveAll(b => b.Id == id);
            return Task.CompletedTask;
        }

        // --- Users ---
        public Task<List<User>> GetUsersAsync() => Task.FromResult(_users.ToList());

        public Task<User?> GetUserAsync(string id) => Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
        public Task<User?> GetUserByUsernameAsync(string username) => Task.FromResult(_users.FirstOrDefault(u => u.Username == username));

        public Task CreateUserAsync(User user)
        {
            user.Id = Guid.NewGuid().ToString();
            _users.Add(user);
            return Task.CompletedTask;
        }

        public Task UpdateUserAsync(string id, User user)
        {
            var existing = _users.FirstOrDefault(u => u.Id == id);
            if (existing != null)
            {
                existing.Username = user.Username;
                existing.PasswordHash = user.PasswordHash;
                existing.Role = user.Role;
            }
            return Task.CompletedTask;
        }

        public Task DeleteUserAsync(string id)
        {
            _users.RemoveAll(u => u.Id == id);
            return Task.CompletedTask;
        }

        // --- BorrowingRequests ---
        public Task<List<BorrowingRequest>> GetRequestsAsync() => Task.FromResult(_requests.ToList());
        public Task<BorrowingRequest?> GetRequestAsync(string id) => Task.FromResult(_requests.FirstOrDefault(r => r.Id == id));
        public Task CreateRequestAsync(BorrowingRequest req) { _requests.Add(req); return Task.CompletedTask; }
        
        public Task ApproveRequestAsync(string id)
        {
            var req = _requests.FirstOrDefault(r => r.Id == id);
            if (req != null)
            {
                req.Status = "Approved";
                _borrowed.Add(new BorrowingBook { 
                    BookTitle = req.BookTitle, 
                    BorrowerName = req.StudentName, 
                    DueDate = req.ReturnDate, 
                    Status = "Active" 
                });
            }
            return Task.CompletedTask;
        }
        
        // --- BorrowingBooks ---
        public Task<List<BorrowingBook>> GetBorrowingBooksAsync() => Task.FromResult(_borrowed.ToList());

        // --- EntryLogs ---
        public Task<List<EntryLog>> GetEntryLogsAsync() => Task.FromResult(_logs.ToList());
        public Task CreateEntryLogAsync(EntryLog log) { _logs.Add(log); return Task.CompletedTask; }
    }
}
