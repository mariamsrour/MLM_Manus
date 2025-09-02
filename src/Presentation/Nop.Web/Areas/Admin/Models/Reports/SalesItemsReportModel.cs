using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Reports;

public partial record SalesItemsReportModel : BaseNopModel
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public string ProductName { get; set; }
    public string VendorName { get; set; }
    public int Quantity { get; set; }
    public string TotalAmount { get; set; }
    public int Rank { get; set; }
}
