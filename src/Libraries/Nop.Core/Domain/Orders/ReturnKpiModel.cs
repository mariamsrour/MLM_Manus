using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Orders;
public partial class ReturnKpiModel
{
    public decimal TotalRefundedOnWallet { get; set; }
    public decimal TotalRefundedOnCard { get; set; }
    public decimal TotalRefundedAmount { get; set; }
}

public partial class ReturnTableCustomerModel
{
    public int OrderID { get; set; }
    public string CustomerName { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string RefundedStatus { get; set; }
    public int RequestId { get; set; }
    public int ReturnedQuantity { get; set; }
    public string Product { get; set; }



}

public partial class ReturnTableVendorModel
{
    public int OrderID { get; set; }
    public int VendorID { get; set; }
    public string VendorName { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public decimal Amount { get; set; }
    public string RefundedStatus { get; set; }
    public int RequestId { get; set; }
    public int ReturnedQuantity { get; set; }
    public string Product { get; set; }
}


