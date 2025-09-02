using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Directory;
using Nop.Data;
using Nop.Services.Directory;

namespace Nop.Services.Customers;
public partial class WalletService: IWalletService
{
    private readonly IRepository<WalletTransaction> _walletTransactionRepository;
    private readonly ICurrencyService _currencyService;

    public WalletService(IRepository<WalletTransaction> walletTransactionRepository, ICurrencyService currencyService)
    {
        _walletTransactionRepository = walletTransactionRepository;
        _currencyService = currencyService;
    }

    public async Task<decimal> GetWalletBalanceAsync(int customerId, string currencyCode)
    {
        // Get transactions in primary currency
        var transactions = await _walletTransactionRepository.Table
            .Where(t => t.CustomerId == customerId)
            .ToListAsync();
        var curr = await _currencyService.GetCurrencyByCodeAsync(currencyCode);

        var balanceInPrimaryCurrency = transactions.Sum(t =>  t.Amount );

        // Convert to requested currency
        return await _currencyService.ConvertFromPrimaryStoreCurrencyAsync(balanceInPrimaryCurrency, curr);
    }

    public async Task AddTransactionAsync(int customerId, decimal amount, string transactionType, string description, string currencyCode)
    {
        var curr = await _currencyService.GetCurrencyByCodeAsync(currencyCode);

        // Convert to primary currency
        var amountInPrimaryCurrency = await _currencyService.ConvertToPrimaryStoreCurrencyAsync(amount, curr);

        var transaction = new WalletTransaction
        {
            CustomerId = customerId,
            Amount = amountInPrimaryCurrency,
            TransactionType = transactionType,
            Description = description,
            CurrencyCode = curr.CurrencyCode,
            CreatedOnUtc = DateTime.UtcNow
        };

        await _walletTransactionRepository.InsertAsync(transaction);
    }

    public async Task<IList<WalletTransaction>> GetTransactionsAsync(int customerId)
    {
        return await _walletTransactionRepository.Table
            .Where(t => t.CustomerId == customerId)
            .OrderByDescending(t => t.CreatedOnUtc)
            .ToListAsync();
    }
}

