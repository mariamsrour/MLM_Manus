using Nop.Core.Domain.Customers;

namespace Nop.Web.Areas.Admin.Models;

public class WalletViewModel
{
    public WalletViewModel()
    {
        Transactions = new List<WalletTransaction>();
    }
    public string Balance { get; set; }

    public List<WalletTransaction> Transactions { get; set; }
}
