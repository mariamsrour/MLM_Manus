using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Reports;

/// <summary>
/// Represents a sales summary list model
/// </summary>
public partial record SalesSummaryListModel : BasePagedListModel<SalesSummaryModel>
{
    public KPISummaryModel KPISummary { get; set; }

}

public class KPISummaryModel
{
    public int TotalCompletedOrders { get; set; }
    public int TotalCanceledOrders { get; set; }
    public int TotalRefundedOrders { get; set; }
}