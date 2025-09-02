using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Reports;

public partial record PeekHourReportModel : BaseNopModel
{
    public int CountryId { get; set; }
    public string CountryName { get; set; }
    public string PeakHour { get; set; } // The hour of the day (0-23)
    public string NumberOfOrders { get; set; }
    public int Rank { get; set; }
}
