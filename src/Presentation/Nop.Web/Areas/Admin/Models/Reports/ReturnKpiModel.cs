using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Reports;

public partial record ReturnKpiModel : BaseNopModel
{
    public string TotalRefundedOnWallet { get; set; }
    public string TotalRefundedOnCard { get; set; }
    public string TotalRefundedAmount { get; set; }
}

public partial record ReturnTableCustomerModel : BaseNopModel
{
    public int OrderID { get; set; }
    public string CustomerName { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string RefundedStatus { get; set; }
    public int RequestId { get; set; }
    public int ReturnedQuantity { get; set; }
    public string Product { get; set; }



}

public partial record ReturnTableCustomerListModel : BasePagedListModel<ReturnTableCustomerModel>
{ }

public partial record ReturnTableCustomerSearchModel : BaseSearchModel
{
    [NopResourceDisplayName("Admin.Reports.Sales.Bestsellers.StartDate")]
    [UIHint("DateNullable")]
    public DateTime? StartDate { get; set; }

    [NopResourceDisplayName("Admin.Reports.Sales.Bestsellers.EndDate")]
    [UIHint("DateNullable")]
    public DateTime? EndDate { get; set; }
}

public partial record ReturnTableVendorModel :BaseNopModel
{
    public int OrderID { get; set; }
    public int VendorID { get; set; }
    public string VendorName { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string Amount { get; set; }
    public string RefundedStatus { get; set; }
    public int RequestId { get; set; }
    public int ReturnedQuantity { get; set; }
    public string Product { get; set; }
}

public partial record ReturnTableVendorListModel : BasePagedListModel<ReturnTableVendorModel>
{ }

public partial record ReturnTableVendorSearchModel : BaseSearchModel
{
    [NopResourceDisplayName("Admin.Reports.Sales.Bestsellers.StartDate")]
    [UIHint("DateNullable")]
    public DateTime? StartDate { get; set; }

    [NopResourceDisplayName("Admin.Reports.Sales.Bestsellers.EndDate")]
    [UIHint("DateNullable")]
    public DateTime? EndDate { get; set; }
}


public class ReturnKPIRequestModel 
{

    public DateTime? start { get; set; }

    public DateTime? end { get; set; }
}