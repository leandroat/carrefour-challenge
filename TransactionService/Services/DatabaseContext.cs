using TransactionService.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace TransactionService.Services
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<TransactionTable> Transaction { get; set; }
    }
}
