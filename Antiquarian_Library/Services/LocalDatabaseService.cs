using Antiquarian_Library.Data;
using Antiquarian_Library.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Antiquarian_Library.Services
{
    public class LocalDatabaseService
    {
        private readonly ApplicationDbContext _context;

        public LocalDatabaseService(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- Books ---
        public async Task<List<Book>> GetBooksAsync() => await _context.Books.ToListAsync();

        public async Task<Book?> GetBookAsync(string id) => await _context.Books.FirstOrDefaultAsync(b => b.Id == id);

        public async Task CreateBookAsync(Book book)
        {
            book.Id = Guid.NewGuid().ToString();
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(string id, Book book)
        {
            var existing = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
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
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteBookAsync(string id)
        {
            var existing = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (existing != null)
            {
                _context.Books.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }

        // --- Users ---
        public async Task<List<User>> GetUsersAsync() => await _context.Users.ToListAsync();

        public async Task<User?> GetUserAsync(string id) => await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        
        public async Task<User?> GetUserByUsernameAsync(string username) => await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        public async Task CreateUserAsync(User user)
        {
            user.Id = Guid.NewGuid().ToString();
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(string id, User user)
        {
            var existing = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (existing != null)
            {
                existing.Username = user.Username;
                existing.PasswordHash = user.PasswordHash;
                existing.Role = user.Role;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteUserAsync(string id)
        {
            var existing = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (existing != null)
            {
                _context.Users.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }

        // --- BorrowingRequests ---
        public async Task<List<BorrowingRequest>> GetRequestsAsync() => await _context.BorrowingRequests.ToListAsync();
        
        public async Task<BorrowingRequest?> GetRequestAsync(string id) => await _context.BorrowingRequests.FirstOrDefaultAsync(r => r.Id == id);
        
        public async Task CreateRequestAsync(BorrowingRequest req) 
        { 
            _context.BorrowingRequests.Add(req); 
            await _context.SaveChangesAsync(); 
        }
        
        public async Task<bool> ApproveRequestAsync(string id)
        {
            var req = await _context.BorrowingRequests.FirstOrDefaultAsync(r => r.Id == id);
            if (req != null && req.Status == "Pending")
            {
                var book = await _context.Books.FirstOrDefaultAsync(b => b.Title == req.BookTitle);
                if (book == null || book.Available <= 0)
                {
                    return false;
                }

                req.Status = "Approved";
                book.Available--;

                var newBorrowing = new BorrowingBook 
                { 
                    BookTitle = req.BookTitle, 
                    BorrowerName = req.StudentName, 
                    DueDate = req.ReturnDate, 
                    Status = "Active" 
                };
                _context.BorrowingBooks.Add(newBorrowing);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RejectRequestAsync(string id)
        {
            var req = await _context.BorrowingRequests.FirstOrDefaultAsync(r => r.Id == id);
            if (req != null && req.Status == "Pending")
            {
                req.Status = "Rejected";
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        
        // --- BorrowingBooks ---
        public async Task<List<BorrowingBook>> GetBorrowingBooksAsync() => await _context.BorrowingBooks.ToListAsync();

        public async Task<bool> ReturnBookAsync(string id)
        {
            var borrowing = await _context.BorrowingBooks.FirstOrDefaultAsync(b => b.Id == id);
            if (borrowing != null && borrowing.Status == "Active")
            {
                borrowing.Status = "Returned";
                
                var book = await _context.Books.FirstOrDefaultAsync(b => b.Title == borrowing.BookTitle);
                if (book != null)
                {
                    book.Available++;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // --- EntryLogs ---
        public async Task<List<EntryLog>> GetEntryLogsAsync() => await _context.EntryLogs.ToListAsync();
        
        public async Task CreateEntryLogAsync(EntryLog log) 
        { 
            _context.EntryLogs.Add(log); 
            await _context.SaveChangesAsync(); 
        }
    }
}
