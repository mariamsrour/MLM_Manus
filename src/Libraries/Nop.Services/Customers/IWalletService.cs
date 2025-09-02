using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Directory;

namespace Nop.Services.Customers;
public partial interface IWalletService
{
    Task<decimal> GetWalletBalanceAsync(int customerId, string currencyCode);
    Task AddTransactionAsync(int customerId, decimal amount, string transactionType, string description, string currencyCode);
    Task<IList<WalletTransaction>> GetTransactionsAsync(int customerId);
}
