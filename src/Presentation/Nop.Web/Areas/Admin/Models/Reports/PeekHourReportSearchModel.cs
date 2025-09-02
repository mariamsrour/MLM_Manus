using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Nop.Web.Areas.Admin.Models.Reports;

public partial record PeekHourReportSearchModel : BaseSearchModel
{
    [UIHint("DateNullable")]
    public DateTime? StartDate { get; set; }
    [UIHint("DateNullable")]
    public DateTime? EndDate { get; set; }
}
