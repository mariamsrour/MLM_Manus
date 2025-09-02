using System.ComponentModel.DataAnnotations;
using Nop.Services.Orders;
using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Reports;

public partial record VendorOrderReportSearchModel : BaseSearchModel
{
    [UIHint("DateNullable")]
    public DateTime? StartDate { get; set; }
    [UIHint("DateNullable")]
    public DateTime? EndDate { get; set; }

}

public partial record CategoryOrderReportSearchModel : BaseSearchModel
{
    [UIHint("DateNullable")]
    public DateTime? StartDate { get; set; }
    [UIHint("DateNullable")]
    public DateTime? EndDate { get; set; }
}
