using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Orders;
public partial class ReturnedItemsPerVendorReportLine
{
    public int Rank { get; set; }
    public int VendorId { get; set; }
    public string VendorName { get; set; }
    public int TotalReturnedItems { get; set; }
    public decimal TotalReturnedAmount { get; set; }
}
