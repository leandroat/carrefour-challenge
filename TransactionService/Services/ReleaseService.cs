using Azure;
using TransactionService.Models;
using TransactionService.Models.Database;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;

namespace TransactionService.Services
{
    public class ReleaseService : IReleaseProvider
    {
        private readonly ILogger<ReleaseService> _logger;
        private readonly DatabaseContext _databaseContext;

        private const string _hashPrefix = "";
        private const int _hashNumberOfSecureBytesToGenerate = 25;
        private const int _hashLengthOfKey = 32;

        public ReleaseService(ILogger<ReleaseService> logger, DatabaseContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }

        public async Task<Transaction> GetTransaction(string id)
        {
            Transaction response = new Transaction();

            try
            {
                var result = await _databaseContext.Transaction.SingleOrDefaultAsync(x => x.Id == id);

                if (result != null)
                {
                    response.Id = result.Id;
                    response.Amount = Convert.ToDouble(result.Amount);
                    response.Description = result.Description;
                    response.Date = DateTime.ParseExact(result.Date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Failed to get transaction ({id}) from database. {ex.Message}");
            }

            return response;
        }


        public async Task<bool> RemoveTransaction(string id)
        {
            try
            {
                var result = await _databaseContext.Transaction.SingleOrDefaultAsync(x => x.Id == id);

                if (result != null)
                {
                    _databaseContext.Transaction.Remove(result);
                    await _databaseContext.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Failed to remove transaction ({id}) from database. {ex.Message}");
            }

            return false;
        }

        public async Task<Transaction> CreateTransaction(Transaction request)
        {
            string amount = request.Amount.ToString();
            string description = request.Description;
            string id = GenerateHashId();

            Transaction response = request;

            TransactionTable insertion = new TransactionTable()
            {
                Amount = amount,
                Description = description,
                Date = GetDate(),
                Id = id
            };

            try
            {
                await _databaseContext.Transaction.AddAsync(insertion);
                var entries = await _databaseContext.SaveChangesAsync();

                if (entries > 0)
                {
                    response.Id = id;
                    response.Date = DateTime.ParseExact(insertion.Date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Failed to save transaction to database. {ex.Message}");
            }

            return response;
        }

        public async Task<Balance> BalanceProcess(DateTime? date)
        {
            Balance response = new Balance()
            {
                Amount = 0,
                Date = null
            };

            try
            {
                if (date != null && date.HasValue)
                {
                    response.Date = date;
                    string? strDate = date?.ToString("dd/MM/yyyy");
                    var result = await _databaseContext.Transaction.Where(t => t.Date == strDate).Select(t => t.Amount).ToListAsync();
                    foreach (var entry in result)
                    {
                        response.Amount += Convert.ToDouble(entry);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Failed to fetch balance from database. {ex.Message}");
            }

            return response;
        }

        private string GenerateHashId()
        {
            var bytes = RandomNumberGenerator.GetBytes(_hashNumberOfSecureBytesToGenerate);

            return string.Concat(_hashPrefix, Convert.ToBase64String(bytes)
                .Replace("/", "")
                .Replace("+", "")
                .Replace("=", "")
                .AsSpan(0, _hashLengthOfKey - _hashPrefix.Length));
        }

        private string GetDate()
        {
            DateTime dateTime = DateTime.UtcNow;
            TimeZoneInfo tzBrazilia = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, tzBrazilia).ToString("dd/MM/yyyy");


        }
    }
}
