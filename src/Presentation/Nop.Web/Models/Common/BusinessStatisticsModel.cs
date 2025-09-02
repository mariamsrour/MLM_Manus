using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Common;

public partial record BusinessStatisticsModel : BaseNopModel
{
    public int TotalUsers { get; set; }

    public int CanceledOrders { get; set; }

    public int ReturnedOrders { get; set; }

    public int DeliveredOrders { get; set; }

    public decimal TotalRevenue { get; set; }

    public decimal TotalVAT { get; set; }

}


public partial record BusinessStatisticsSearchModel : BaseSearchModel
{
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

}
