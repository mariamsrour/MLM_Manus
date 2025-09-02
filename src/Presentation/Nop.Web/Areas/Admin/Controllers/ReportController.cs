using Microsoft.AspNetCore.Mvc;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.Reports;

namespace Nop.Web.Areas.Admin.Controllers;

public partial class ReportController : BaseAdminController
{
    #region Fields

    protected readonly IPermissionService _permissionService;
    protected readonly IReportModelFactory _reportModelFactory;

    #endregion

    #region Ctor

    public ReportController(
        IPermissionService permissionService,
        IReportModelFactory reportModelFactory)
    {
        _permissionService = permissionService;
        _reportModelFactory = reportModelFactory;
    }

    #endregion

    #region Methods

    #region Sales summary

    public virtual async Task<IActionResult> SalesSummary(List<int> orderStatuses = null, List<int> paymentStatuses = null)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.SalesSummaryReport))
            return AccessDeniedView();

        //prepare model
        var model = await _reportModelFactory.PrepareSalesSummarySearchModelAsync(new SalesSummarySearchModel
        {
            OrderStatusIds = orderStatuses,
            PaymentStatusIds = paymentStatuses
        });
        var data = await _reportModelFactory.PrepareSalesSummaryListModelAsync(new SalesSummarySearchModel
        {
            OrderStatusIds = orderStatuses,
            PaymentStatusIds = paymentStatuses
        });
        //var totalCompletedOrders = data.Data.Where(m => m.Summary == OrderStatus.Complete).Sum(m => m.NumberOfOrders);
        //var totalCanceledOrders = data.Data.Where(m => m.OrderStatus == OrderStatus.Cancelled).Sum(m => m.NumberOfOrders);
        //var totalRefundedOrders = data.Data.Where(m => m.OrderStatus == OrderStatus.Refunded).Sum(m => m.NumberOfOrders);

        //var kpiModel = new KPISummaryModel
        //{
        //    TotalCompletedOrders = totalCompletedOrders,
        //    TotalCanceledOrders = totalCanceledOrders,
        //    TotalRefundedOrders = totalRefundedOrders
        //};
        //data.KPISummary = kpiModel;
        ViewBag.chart1data1 =data.Data.Select(x=>x.NumberOfOrders);
        ViewBag.chart1data2 = data.Data.Select(x => decimal.Parse(x.OrderTotal.Replace("$",string.Empty)));
        ViewBag.chart1data3 = data.Data.Select(x => decimal.Parse(x.ProfitStr.Replace("$", string.Empty)));
        ViewBag.chart1data4 = data.Data.Select(x => decimal.Parse(x.Shipping.Replace("$", string.Empty)));
        ViewBag.chart1title =data.Data.Select(x=>x.Summary);

        //ViewBag.chart2data1 = data.Data.Select(x => x.NumberOfOrders);
        //ViewBag.chart2data2 = data.Data.Select(x => x.OrderTotal);
        //ViewBag.chart2data3 = data.Data.Select(x => x.ProfitStr);
        //ViewBag.chart2data4 = data.Data.Select(x => x.Shipping);
        //ViewBag.chart2title = data.Data.Select(x => x.Summary);

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> SalesSummaryList(SalesSummarySearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.SalesSummaryReport))
            return await AccessDeniedDataTablesJson();

        //prepare model
        var model = await _reportModelFactory.PrepareSalesSummaryListModelAsync(searchModel);
        ViewBag.chart1data1 = model.Data.Select(x => x.NumberOfOrders);
        ViewBag.chart1data2 = model.Data.Select(x => decimal.Parse(x.OrderTotal.Replace("$",string.Empty)));
        ViewBag.chart1data3 = model.Data.Select(x => decimal.Parse(x.ProfitStr.Replace("$", string.Empty)));
        ViewBag.chart1data4 = model.Data.Select(x => decimal.Parse(x.Shipping.Replace("$", string.Empty)));
        ViewBag.chart1title = model.Data.Select(x => x.Summary);

        return Json(model);
    }


    #endregion

    #region Low stock

    public virtual async Task<IActionResult> LowStock()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
            return AccessDeniedView();

        //prepare model
        var model = await _reportModelFactory.PrepareLowStockProductSearchModelAsync(new LowStockProductSearchModel());

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> LowStockList(LowStockProductSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
            return await AccessDeniedDataTablesJson();

        //prepare model
        var model = await _reportModelFactory.PrepareLowStockProductListModelAsync(searchModel);

        return Json(model);
    }

    #endregion

    #region Bestsellers

    public virtual async Task<IActionResult> Bestsellers()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
            return AccessDeniedView();

        //prepare model
        var model = await _reportModelFactory.PrepareBestsellerSearchModelAsync(new BestsellerSearchModel());

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> BestsellersList(BestsellerSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
            return await AccessDeniedDataTablesJson();

        //prepare model
        var model = await _reportModelFactory.PrepareBestsellerListModelAsync(searchModel);

        return Json(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> BestsellersReportAggregates(BestsellerSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
            return await AccessDeniedDataTablesJson();

        //prepare model
        var totalAmount = await _reportModelFactory.GetBestsellerTotalAmountAsync(searchModel);

        return Json(new { aggregatortotal = totalAmount });
    }

    #endregion

    #region Never Sold

    public virtual async Task<IActionResult> NeverSold()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
            return AccessDeniedView();

        //prepare model
        var model = await _reportModelFactory.PrepareNeverSoldSearchModelAsync(new NeverSoldReportSearchModel());

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> NeverSoldList(NeverSoldReportSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
            return await AccessDeniedDataTablesJson();

        //prepare model
        var model = await _reportModelFactory.PrepareNeverSoldListModelAsync(searchModel);

        return Json(model);
    }

    #endregion

    #region Country sales

    public virtual async Task<IActionResult> CountrySales()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.OrderCountryReport))
            return AccessDeniedView();

        //prepare model
        var model = await _reportModelFactory.PrepareCountrySalesSearchModelAsync(new CountryReportSearchModel());

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> CountrySalesList(CountryReportSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.OrderCountryReport))
            return await AccessDeniedDataTablesJson();

        //prepare model
        var model = await _reportModelFactory.PrepareCountrySalesListModelAsync(searchModel);

        return Json(model);
    }

    #endregion

    #region Customer reports

    public virtual async Task<IActionResult> RegisteredCustomers()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
            return AccessDeniedView();

        //prepare model
        var model = await _reportModelFactory.PrepareCustomerReportsSearchModelAsync(new CustomerReportsSearchModel());

        return View(model);
    }

    public virtual async Task<IActionResult> BestCustomersByOrderTotal()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
            return AccessDeniedView();

        //prepare model
        var model = await _reportModelFactory.PrepareCustomerReportsSearchModelAsync(new CustomerReportsSearchModel());

        return View(model);
    }

    public virtual async Task<IActionResult> BestCustomersByNumberOfOrders()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
            return AccessDeniedView();

        //prepare model
        var model = await _reportModelFactory.PrepareCustomerReportsSearchModelAsync(new CustomerReportsSearchModel());

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> ReportBestCustomersByOrderTotalList(BestCustomersReportSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
            return await AccessDeniedDataTablesJson();

        //prepare model
        var model = await _reportModelFactory.PrepareBestCustomersReportListModelAsync(searchModel);

        return Json(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> ReportBestCustomersByNumberOfOrdersList(BestCustomersReportSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
            return await AccessDeniedDataTablesJson();

        //prepare model
        var model = await _reportModelFactory.PrepareBestCustomersReportListModelAsync(searchModel);

        return Json(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> ReportRegisteredCustomersList(RegisteredCustomersReportSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
            return await AccessDeniedDataTablesJson();

        //prepare model
        var model = await _reportModelFactory.PrepareRegisteredCustomersReportListModelAsync(searchModel);

        return Json(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> ReportGenderStatisticsList([FromBody] RegisteredCustomersReportSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
            return await AccessDeniedDataTablesJson();

        //prepare model
        var model = await _reportModelFactory.PrepareGenderCustomersReportListModelAsync(searchModel);

        return Json(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> ReportUsersinYearList([FromBody] RegisteredCustomersReportSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
            return await AccessDeniedDataTablesJson();

        //prepare model
        var model = await _reportModelFactory.PrepareUsersinYearReportListModelAsync(searchModel);

        return Json(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> ReportUsersInMonthList([FromBody] RegisteredCustomersReportSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCustomers))
            return await AccessDeniedDataTablesJson();

        //prepare model
        var model = await _reportModelFactory.PrepareUsersinMonthReporListModelAsync(searchModel);

        return Json(model);
    }
    #endregion

    #region bestsellersCategory

    public virtual async Task<IActionResult> BestsellersCategories()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
            return AccessDeniedView();

        //prepare model
        var model = await _reportModelFactory.PrepareBestsellerCategorySearchModelAsync(new CategoryOrderReportSearchModel());

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> BestsellersCategories(CategoryOrderReportSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
            return await AccessDeniedDataTablesJson();

        //prepare model
        var model = await _reportModelFactory.PrepareBestsellerCategoryListModelAsync(searchModel);

        return Json(model);
    }

    #endregion
  
    #region bestseller vendors

    public virtual async Task<IActionResult> BestsellersVendors()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
            return AccessDeniedView();

        //prepare model
        var model = await _reportModelFactory.PrepareBestsellerVendorSearchModelAsync(new VendorOrderReportSearchModel());

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> BestsellersVendors(VendorOrderReportSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
            return await AccessDeniedDataTablesJson();

        //prepare model
        var model = await _reportModelFactory.PrepareBestsellerVendorListModelAsync(searchModel);

        return Json(model);
    }
    #endregion

    #region peek hour
    public virtual async Task<IActionResult> CountrySalesPeekHours()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
            return AccessDeniedView();

        //prepare model
        var model = await _reportModelFactory.PreparePeekHourSearchModelAsync(new PeekHourReportSearchModel());

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> CountrySalesPeekHours(PeekHourReportSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
            return await AccessDeniedDataTablesJson();

        //prepare model
        var model = await _reportModelFactory.PreparePeekHourListModelAsync(searchModel);

        return Json(model);
    }
    #endregion

    #region Sales Items
    public virtual async Task<IActionResult> SalesItemsSummary()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
            return AccessDeniedView();

        //prepare model
        var model = await _reportModelFactory.PrepareSalesItemsSearchModelAsync(new SalesItemsReportSearchModel());

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> SalesItemsSummary(SalesItemsReportSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
            return await AccessDeniedDataTablesJson();

        //prepare model
        var model = await _reportModelFactory.PrepareSalesItemsListModelAsync(searchModel);

        return Json(model);
    }

    [HttpGet]
    public async Task<IActionResult> EarningsReport(DateTime? startDate, DateTime? endDate)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
            return AccessDeniedView();

        var report = await _reportModelFactory.GetEarningsReportAsync(startDate, endDate);
        return View(report);
    }

    #endregion

    #region Returned Items

    public virtual async Task<IActionResult> VendorsReturnedItems()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
            return AccessDeniedView();

        //prepare model
        var model = await _reportModelFactory.PrepareVendorReturnsSearchModelAsync(new ReturnedItemsPerVendorReportSearchModel());

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> VendorsReturnedItems(ReturnedItemsPerVendorReportSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
            return await AccessDeniedDataTablesJson();

        //prepare model
        var model = await _reportModelFactory.PrepareVendorReturnsListModelAsync(searchModel);

        return Json(model);
    }


    public virtual async Task<IActionResult> CustomerRefundsReport()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
            return AccessDeniedView();

        //prepare model
        var model = await _reportModelFactory.PrepareReturnTableCustomerSearchModelAsync(new ReturnTableCustomerSearchModel());

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> CustomerRefundsReport(ReturnTableCustomerSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
            return await AccessDeniedDataTablesJson();

        //prepare model
        var model = await _reportModelFactory.PrepareReturnTableCustomerListModelAsync(searchModel);

        return Json(model);
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public virtual async Task<IActionResult> RefundsKpisReport([FromBody] ReturnKPIRequestModel model)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
            return AccessDeniedView();

        var report = await _reportModelFactory.GetRefundsKpisAsync(model.start, model.end);
        return Json(report);
    }

    public virtual async Task<IActionResult> VendorsRefundsReport()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
            return AccessDeniedView();

        //prepare model
        var model = await _reportModelFactory.PrepareVendorReturnsTableSearchModelAsync(new ReturnTableVendorSearchModel());

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> VendorsRefundsReport(ReturnTableVendorSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
            return await AccessDeniedDataTablesJson();

        //prepare model
        var model = await _reportModelFactory.PrepareReturnTableVendorListModelAsync(searchModel, false);

        return Json(model);
    }


    public virtual async Task<IActionResult> VendorsClaimedRefundsReport()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
            return AccessDeniedView();

        //prepare model
        var model = await _reportModelFactory.PrepareVendorReturnsTableSearchModelAsync(new ReturnTableVendorSearchModel());

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> VendorsClaimedRefundsReport(ReturnTableVendorSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
            return await AccessDeniedDataTablesJson();

        //prepare model
        var model = await _reportModelFactory.PrepareReturnTableVendorListModelAsync(searchModel, true);

        return Json(model);
    }


    #endregion

    #endregion
}