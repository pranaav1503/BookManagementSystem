using Microsoft.EntityFrameworkCore;
using BookManagementSystem.Models;

namespace BookManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    }
}
