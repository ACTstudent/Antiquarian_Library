using Antiquarian_Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Antiquarian_Library.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BorrowingRequest> BorrowingRequests { get; set; }
        public DbSet<EntryLog> EntryLogs { get; set; }
        public DbSet<BorrowingBook> BorrowingBooks { get; set; }
    }
}
