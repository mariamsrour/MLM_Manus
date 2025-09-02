using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Customers;
public class WalletTransaction : BaseEntity
{
    public int CustomerId { get; set; } // Wallet owner
    public decimal Amount { get; set; } // Transaction amount
    public string TransactionType { get; set; } // Credit/Debit/Refund/TopUp
    public string Description { get; set; } // Transaction details
    public string CurrencyCode { get; set; } // Currency (e.g., USD, EUR)
    public DateTime CreatedOnUtc { get; set; } // Transaction date

    // Navigation property
    public virtual Customer Customer { get; set; }
}
