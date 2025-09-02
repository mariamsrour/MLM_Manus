using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Reports;

public partial record class ReturnedItemsPerVendorReportModel: BaseNopModel
{
    public int Rank { get; set; }

    public int VendorId { get; set; }
    public string VendorName { get; set; }
    public int TotalReturnedItems { get; set; }
    public string TotalReturnedAmount { get; set; }
}
