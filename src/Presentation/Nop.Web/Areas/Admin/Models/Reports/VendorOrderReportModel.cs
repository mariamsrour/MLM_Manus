using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Reports;

public partial record CategoryOrderReportModel : BaseNopModel
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public int NumberOfOrders { get; set; }
    public int Rank { get; set; }

}

public partial record VendorOrderReportModel : BaseNopModel
{

    public int Rank { get; set; }
    public int VendorId { get; set; }
    public string VendorName { get; set; }
    public int NumberOfOrders { get; set; }

}