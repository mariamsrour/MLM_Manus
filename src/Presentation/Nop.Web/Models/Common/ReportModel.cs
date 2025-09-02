using Nop.Core.Domain.Common;
using Nop.Web.Framework.Models;
using Nop.Web.Models.Catalog;

namespace Nop.Web.Models.Common;

public partial record ReportModel : BaseNopEntityModel
{
    public ReportModel()
    {
        AdReportReasons = new List<AdReportReason>();
        Ad = new ProductDetailsModel();
    }
    public ProductDetailsModel Ad { get; set; }
    public List<AdReportReason> AdReportReasons { get; set; }

    public int SelectedReasonId { get; set; }
    public string CustomReason { get; set; }
    public int AdId { get; set; }

}
