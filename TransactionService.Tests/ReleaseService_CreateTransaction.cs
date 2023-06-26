using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Moq;
using TransactionService.Models;
using TransactionService.Services;
using TransactionService.Services.Mock;

namespace TransactionService.Tests;

public class ReleaseService_CreateTransaction
{
    private readonly ILogger<ReleaseService> _logger;
    private readonly DatabaseContextMock _dbContext;

    private readonly Transaction _transactionRequestSucess = new Transaction()
    {
        Amount = 10.0,
        Date = DateTime.UtcNow,
        Description = "111 test transaction"
    };

    public ReleaseService_CreateTransaction()
    {
        _logger = Mock.Of<ILogger<ReleaseService>>();
        _dbContext = new DatabaseContextMock();
    }

    [Fact]
    public async Task CreateTransaction_Success()
    {
        var dbContext = await _dbContext.GetDatabaseContextMock();

        var service = new ReleaseService(_logger, dbContext);

        var created = await service.CreateTransaction(_transactionRequestSucess);

        Assert.NotNull(created);
        Assert.False(string.IsNullOrEmpty(created.Id));

        var retrieved = await service.GetTransaction(created.Id);
        Assert.NotNull(retrieved);
        Assert.Equal(_transactionRequestSucess.Amount, retrieved.Amount);
    }

    [Fact]
    public async Task GetTransaction_Fail()
    {
        var dbContext = await _dbContext.GetDatabaseContextMock();

        var service = new ReleaseService(_logger, dbContext);

        string wrongId = "INVALID_ID";
        var retrieved = await service.GetTransaction(wrongId);
        
        Assert.NotNull(retrieved);

        Assert.Null(retrieved.Id);
        Assert.Null(retrieved.Date);
        Assert.Null(retrieved.Description);
        Assert.Equal(0.0, retrieved.Amount);
    }

    [Fact]
    public async Task RemoveTransaction_Success()
    {
        var dbContext = await _dbContext.GetDatabaseContextMock();

        var service = new ReleaseService(_logger, dbContext);

        var retrieved = await service.GetTransaction(_transactionRequestSucess.Id);
               
        if(retrieved != null && !string.IsNullOrEmpty(retrieved.Id))
        {
            var removed = await service.RemoveTransaction(retrieved.Id);
            Assert.True(removed);
        }
    }

    [Fact]
    public async Task RemoveTransaction_Fail()
    {
        var dbContext = await _dbContext.GetDatabaseContextMock();

        var service = new ReleaseService(_logger, dbContext);

        string wrongId = "INVALID_ID";
        var removed = await service.RemoveTransaction(wrongId);
        Assert.False(removed);
    }

    [Fact]
    public async Task BalanceProcess_Calculate()
    {
        var dbContext = await _dbContext.GetDatabaseContextMock();

        var service = new ReleaseService(_logger, dbContext);

        var retrieved = await service.BalanceProcess(DateTime.Now);

        Assert.NotNull(retrieved);
        Assert.Equal(DateTime.Now.ToString("dd/MM/yyyy"), retrieved.Date?.ToString("dd/MM/yyyy"));
        Assert.True(retrieved.Amount > 0);
    }
}