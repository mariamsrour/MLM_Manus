using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Reports;

/// <summary>
/// Represents a registered customers report search model
/// </summary>
public partial record RegisteredCustomersReportSearchModel : BaseSearchModel
{
    [NopResourceDisplayName("Admin.Reports.Customers.BestBy.StartDate")]
    [UIHint("DateNullable")]
    public DateTime? StartDate { get; set; }

    [NopResourceDisplayName("Admin.Reports.Customers.BestBy.EndDate")]
    [UIHint("DateNullable")]
    public DateTime? EndDate { get; set; }
}