using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Common;

public partial record DeliveryStatisticsModel : BaseNopModel
{
    public int TodayOrders { get; set; }

    public int CompletedOrders { get; set; }

    public int PendingOrders { get; set; }

    public int OutOfDelivery { get; set; }

    public int PreparingOrders { get; set; }

    public int ReadyToDeliver { get; set; }

}


public partial record DeliveryStatisticsSearchModel : BaseSearchModel
{
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

}
