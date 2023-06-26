using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TransactionService.Models.Database;

namespace TransactionService.Services.Mock
{
    public class DatabaseContextMock
    {
        private readonly List<TransactionTable> _dbEntries = new List<TransactionTable>()
        {
            new TransactionTable()
            {
                Id = "AAA",
                Amount = (10.0).ToString(),
                Date = DateTime.Now.ToString("dd/MM/yyyy"),
                Description = "AAA Transaction"
            },
            new TransactionTable()
            {
                Id = "BBB",
                Amount = (200.0).ToString(),
                Date = DateTime.Now.ToString("dd/MM/yyyy"),
                Description = "BBB Transaction"
            },
            new TransactionTable()
            {
                Id = "CCC",
                Amount = (-100.0).ToString(),
                Date = DateTime.Now.ToString("dd/MM/yyyy"),
                Description = "CCC Transaction"
            },
            new TransactionTable()
            {
                Id = "DDD",
                Amount = (100.0).ToString(),
                Date = DateTime.Now.ToString("dd/MM/yyyy"),
                Description = "DDD Transaction"
            },
            new TransactionTable()
            {
                Id = "EEE",
                Amount = (-50.0).ToString(),
                Date = DateTime.Now.ToString("dd/MM/yyyy"),
                Description = "EEE Transaction"
            }
        };

        public async Task<DatabaseContext> GetDatabaseContextMock()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new DatabaseContext(options);
            databaseContext.Database.EnsureCreated();

            if (await databaseContext.Transaction.CountAsync() <= 0)
            {
                foreach(var entry in _dbEntries)
                {
                    databaseContext.Transaction.Add(new TransactionTable()
                    {
                        Id = entry.Id,
                        Amount = entry.Amount,
                        Date = entry.Date,
                        Description = entry.Description
                    });
                    await databaseContext.SaveChangesAsync();
                }
            }

            return databaseContext;
        }
    }
}
