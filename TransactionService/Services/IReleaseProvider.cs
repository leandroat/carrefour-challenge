using TransactionService.Models;

namespace TransactionService.Services
{
    public interface IReleaseProvider
    {
        Task<Transaction> CreateTransaction(Transaction request);

        Task<Transaction> GetTransaction(string id);

        Task<bool> RemoveTransaction(string id);

        Task<Balance> BalanceProcess(DateTime? date);
    }
}
