using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Orders;
public partial class EarningsReportModel
{
    public int TotalOrders { get; set; }
    public decimal TotalOrderValue { get; set; }
    public decimal TotalOrderCost { get; set; }
    public decimal TotalProductsPrice { get; set; }
    public int TotalProductsSold { get; set; }
    public decimal TotalShippingFees { get; set; }
    public decimal TotalTaxes { get; set; }
    public int TotalCouponsUsed { get; set; }
    public decimal TotalCouponsValue { get; set; }
    public int TotalReturnedProducts { get; set; }
    public decimal TotalReturnsValue { get; set; }
}
