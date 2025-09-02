using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Orders;
public partial class SalesItemsReportLine
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public string ProductName { get; set; }
    public string VendorName { get; set; }
    public int Quantity { get; set; }
    public decimal TotalAmount { get; set; }
    public int Rank { get; set; }


}
