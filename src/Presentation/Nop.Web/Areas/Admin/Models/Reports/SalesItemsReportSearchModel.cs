using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Reports;

public partial record SalesItemsReportSearchModel : BaseSearchModel
{
    [UIHint("DateNullable")]
    public DateTime? StartDate { get; set; }
    [UIHint("DateNullable")]
    public DateTime? EndDate { get; set; }
}
