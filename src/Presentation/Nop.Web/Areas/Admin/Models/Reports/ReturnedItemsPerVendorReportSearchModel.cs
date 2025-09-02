using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Reports;

public partial record class ReturnedItemsPerVendorReportSearchModel : BaseSearchModel
{
    public ReturnedItemsPerVendorReportSearchModel()
    {
        AvailableVendors = new List<SelectListItem>();
    }

    [NopResourceDisplayName("Admin.Reports.Customers.BestBy.StartDate")]
    [UIHint("DateNullable")]
    public DateTime? StartDate { get; set; }

    [NopResourceDisplayName("Admin.Reports.Customers.BestBy.EndDate")]
    [UIHint("DateNullable")]
    public DateTime? EndDate { get; set; }

    [NopResourceDisplayName("Admin.Reports.SalesSummary.Vendor")]
    public int VendorId { get; set; }
    public IList<SelectListItem> AvailableVendors { get; set; }

}
