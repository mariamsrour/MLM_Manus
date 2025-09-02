using System.Globalization;
using System.Numerics;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Vendors;
using Nop.Data;
using Nop.Services.Catalog;
using Nop.Services.Directory;
using Nop.Services.Helpers;
using Nop.Services.Stores;

namespace Nop.Services.Orders;

/// <summary>
/// Order report service
/// </summary>
public partial class OrderReportService : IOrderReportService
{
    #region Fields

    protected readonly CurrencySettings _currencySettings;
    protected readonly ICurrencyService _currencyService;
    protected readonly IDateTimeHelper _dateTimeHelper;
    protected readonly IPriceFormatter _priceFormatter;
    protected readonly IRepository<Address> _addressRepository;
    protected readonly IRepository<Order> _orderRepository;
    protected readonly IRepository<OrderItem> _orderItemRepository;
    protected readonly IRepository<OrderNote> _orderNoteRepository;
    protected readonly IRepository<Product> _productRepository;
    protected readonly IRepository<ProductCategory> _productCategoryRepository;
    protected readonly IRepository<Category> _categoryRepository;
    protected readonly IRepository<ProductManufacturer> _productManufacturerRepository;
    protected readonly IRepository<ProductWarehouseInventory> _productWarehouseInventoryRepository;
    protected readonly IRepository<Vendor> _vendorRepository;
    protected readonly IRepository<Country> _countryRepository;
    protected readonly IRepository<ReturnRequest> _returnRequestRepository;
    protected readonly IRepository<Discount> _discountsRepository;
    protected readonly IRepository<DiscountProductMapping> _discountProductMappingRepository;
    protected readonly IRepository<Shipment> _shipmentRepository;
    protected readonly IRepository<Customer> _customerRepository;

    protected readonly IStoreMappingService _storeMappingService;
    protected readonly IWorkContext _workContext;

    #endregion

    #region Ctor

    public OrderReportService(
        CurrencySettings currencySettings,
        ICurrencyService currencyService,
        IDateTimeHelper dateTimeHelper,
        IPriceFormatter priceFormatter,
        IRepository<Address> addressRepository,
        IRepository<Order> orderRepository,
        IRepository<OrderItem> orderItemRepository,
        IRepository<OrderNote> orderNoteRepository,
        IRepository<Product> productRepository,
        IRepository<ProductCategory> productCategoryRepository,
        IRepository<ProductManufacturer> productManufacturerRepository,
        IRepository<ProductWarehouseInventory> productWarehouseInventoryRepository,
        IRepository<Category> categoryRepository,
        IStoreMappingService storeMappingService,
        IRepository<Vendor> vendorRepository,
        IRepository<Country> countryRepository,
        IRepository<ReturnRequest> returnRequestRepository,
        IRepository<Discount> discountsRepository,
        IRepository<DiscountProductMapping> discountProductMappingRepository,
        IRepository<Shipment> shipmentRepository,
        IRepository<Customer> customerRepository,
        IWorkContext workContext)
    {
        _currencySettings = currencySettings;
        _currencyService = currencyService;
        _dateTimeHelper = dateTimeHelper;
        _priceFormatter = priceFormatter;
        _addressRepository = addressRepository;
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
        _orderNoteRepository = orderNoteRepository;
        _productRepository = productRepository;
        _productCategoryRepository = productCategoryRepository;
        _productManufacturerRepository = productManufacturerRepository;
        _productWarehouseInventoryRepository = productWarehouseInventoryRepository;
        _storeMappingService = storeMappingService;
        _workContext = workContext;
        _categoryRepository = categoryRepository;
        _vendorRepository = vendorRepository;
        _countryRepository = countryRepository;
        _returnRequestRepository = returnRequestRepository;
        _discountsRepository = discountsRepository;
        _discountProductMappingRepository = discountProductMappingRepository;
        _shipmentRepository = shipmentRepository;
        _customerRepository = customerRepository;
    }

    #endregion

    #region Utilities

    /// <summary>
    /// Search order items
    /// </summary>
    /// <param name="storeId">Store identifier (orders placed in a specific store); 0 to load all records</param>
    /// <param name="vendorId">Vendor identifier; 0 to load all records</param>
    /// <param name="categoryId">Category identifier; 0 to load all records</param>
    /// <param name="manufacturerId">Manufacturer identifier; 0 to load all records</param>
    /// <param name="createdFromUtc">Order created date from (UTC); null to load all records</param>
    /// <param name="createdToUtc">Order created date to (UTC); null to load all records</param>
    /// <param name="os">Order status; null to load all records</param>
    /// <param name="ps">Order payment status; null to load all records</param>
    /// <param name="ss">Shipping status; null to load all records</param>
    /// <param name="billingCountryId">Billing country identifier; 0 to load all records</param>
    /// <param name="showHidden">A value indicating whether to show hidden records</param>
    /// <returns>Result query</returns>
    protected virtual IQueryable<OrderItem> SearchOrderItems(
        int categoryId = 0,
        int manufacturerId = 0,
        int storeId = 0,
        int vendorId = 0,
        DateTime? createdFromUtc = null,
        DateTime? createdToUtc = null,
        OrderStatus? os = null,
        PaymentStatus? ps = null,
        ShippingStatus? ss = null,
        int billingCountryId = 0,
        bool showHidden = false)
    {
        int? orderStatusId = null;
        if (os.HasValue)
            orderStatusId = (int)os.Value;

        int? paymentStatusId = null;
        if (ps.HasValue)
            paymentStatusId = (int)ps.Value;

        int? shippingStatusId = null;
        if (ss.HasValue)
            shippingStatusId = (int)ss.Value;

        var orderItems = from orderItem in _orderItemRepository.Table
                         join o in _orderRepository.Table on orderItem.OrderId equals o.Id
                         join p in _productRepository.Table on orderItem.ProductId equals p.Id
                         join oba in _addressRepository.Table on o.BillingAddressId equals oba.Id
                         where (storeId == 0 || storeId == o.StoreId) &&
                               (!createdFromUtc.HasValue || createdFromUtc.Value <= o.CreatedOnUtc) &&
                               (!createdToUtc.HasValue || createdToUtc.Value >= o.CreatedOnUtc) &&
                               (!orderStatusId.HasValue || orderStatusId == o.OrderStatusId) &&
                               (!paymentStatusId.HasValue || paymentStatusId == o.PaymentStatusId) &&
                               (!shippingStatusId.HasValue || shippingStatusId == o.ShippingStatusId) &&
                               !o.Deleted && !p.Deleted &&
                               (vendorId == 0 || p.VendorId == vendorId) &&
                               (billingCountryId == 0 || oba.CountryId == billingCountryId) &&
                               (showHidden || p.Published)
                         select orderItem;

        if (categoryId > 0)
        {
            orderItems = from orderItem in orderItems
                         join p in _productRepository.Table on orderItem.ProductId equals p.Id
                         join pc in _productCategoryRepository.Table on p.Id equals pc.ProductId
                             into p_pc
                         from pc in p_pc.DefaultIfEmpty()
                         where pc.CategoryId == categoryId
                         select orderItem;
        }

        if (manufacturerId > 0)
        {
            orderItems = from orderItem in orderItems
                         join p in _productRepository.Table on orderItem.ProductId equals p.Id
                         join pm in _productManufacturerRepository.Table on p.Id equals pm.ProductId
                             into p_pm
                         from pm in p_pm.DefaultIfEmpty()
                         where pm.ManufacturerId == manufacturerId
                         select orderItem;
        }

        return orderItems;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Get "order by country" report
    /// </summary>
    /// <param name="storeId">Store identifier</param>
    /// <param name="os">Order status</param>
    /// <param name="ps">Payment status</param>
    /// <param name="ss">Shipping status</param>
    /// <param name="startTimeUtc">Start date</param>
    /// <param name="endTimeUtc">End date</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the result
    /// </returns>
    public virtual async Task<IList<OrderByCountryReportLine>> GetCountryReportAsync(int storeId, OrderStatus? os,
        PaymentStatus? ps, ShippingStatus? ss, DateTime? startTimeUtc, DateTime? endTimeUtc)
    {
        int? orderStatusId = null;
        if (os.HasValue)
            orderStatusId = (int)os.Value;

        int? paymentStatusId = null;
        if (ps.HasValue)
            paymentStatusId = (int)ps.Value;

        int? shippingStatusId = null;
        if (ss.HasValue)
            shippingStatusId = (int)ss.Value;

        var query = _orderRepository.Table;
        query = query.Where(o => !o.Deleted);
        if (storeId > 0)
            query = query.Where(o => o.StoreId == storeId);
        if (orderStatusId.HasValue)
            query = query.Where(o => o.OrderStatusId == orderStatusId.Value);
        if (paymentStatusId.HasValue)
            query = query.Where(o => o.PaymentStatusId == paymentStatusId.Value);
        if (shippingStatusId.HasValue)
            query = query.Where(o => o.ShippingStatusId == shippingStatusId.Value);
        if (startTimeUtc.HasValue)
            query = query.Where(o => startTimeUtc.Value <= o.CreatedOnUtc);
        if (endTimeUtc.HasValue)
            query = query.Where(o => endTimeUtc.Value >= o.CreatedOnUtc);

        //var report = await (from oq in query
        //        join a in _addressRepository.Table on oq.BillingAddressId equals a.Id
        //        group oq by a.CountryId 
        //        into result
        //        select new
        //        {
        //            CountryId = result.Key,
        //            TotalOrders = result.Count(),
        //            SumOrders = result.Sum(o => o.OrderTotal)
        //        })
        //    .OrderByDescending(x => x.SumOrders)
        //    .Select(r => new OrderByCountryReportLine
        //    {
        //        CountryId = r.CountryId,
        //        TotalOrders = r.TotalOrders,
        //        SumOrders = r.SumOrders
        //    })
        //    .ToListAsync();
        var report = await (from oq in query
                            join a in _addressRepository.Table on oq.BillingAddressId equals a.Id
                            group oq by new { a.CountryId, a.City } // Group by both CountryId and City
                    into result
                            select new
                            {
                                CountryId = result.Key.CountryId,
                                City = result.Key.City, // Include City in the grouping
                                TotalOrders = result.Count(),
                                SumOrders = result.Sum(o => o.OrderTotal)
                            })
    .OrderByDescending(x => x.SumOrders)
    .Select(r => new OrderByCountryReportLine
    {
        CountryId = r.CountryId,
        City = r.City, // Include City in the output
        TotalOrders = r.TotalOrders,
        SumOrders = r.SumOrders
    })
    .ToListAsync();

        return report;
    }

    /// <summary>
    /// Get order average report
    /// </summary>
    /// <param name="storeId">Store identifier; pass 0 to ignore this parameter</param>
    /// <param name="vendorId">Vendor identifier; pass 0 to ignore this parameter</param>
    /// <param name="productId">Product identifier which was purchased in an order; 0 to load all orders</param>
    /// <param name="warehouseId">Warehouse identifier; pass 0 to ignore this parameter</param>
    /// <param name="billingCountryId">Billing country identifier; 0 to load all orders</param>
    /// <param name="orderId">Order identifier; pass 0 to ignore this parameter</param>
    /// <param name="paymentMethodSystemName">Payment method system name; null to load all records</param>
    /// <param name="osIds">Order status identifiers</param>
    /// <param name="psIds">Payment status identifiers</param>
    /// <param name="ssIds">Shipping status identifiers</param>
    /// <param name="startTimeUtc">Start date</param>
    /// <param name="endTimeUtc">End date</param>
    /// <param name="billingPhone">Billing phone. Leave empty to load all records.</param>
    /// <param name="billingEmail">Billing email. Leave empty to load all records.</param>
    /// <param name="billingLastName">Billing last name. Leave empty to load all records.</param>
    /// <param name="orderNotes">Search in order notes. Leave empty to load all records.</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the result
    /// </returns>
    public virtual async Task<OrderAverageReportLine> GetOrderAverageReportLineAsync(int storeId = 0,
        int vendorId = 0, int productId = 0, int warehouseId = 0, int billingCountryId = 0,
        int orderId = 0, string paymentMethodSystemName = null,
        List<int> osIds = null, List<int> psIds = null, List<int> ssIds = null,
        DateTime? startTimeUtc = null, DateTime? endTimeUtc = null,
        string billingPhone = null, string billingEmail = null, string billingLastName = "", string orderNotes = null)
    {
        var query = _orderRepository.Table;

        query = query.Where(o => !o.Deleted);
        if (storeId > 0)
            query = query.Where(o => o.StoreId == storeId);
        if (orderId > 0)
            query = query.Where(o => o.Id == orderId);

        if (vendorId > 0)
        {
            query = from o in query
                    join oi in _orderItemRepository.Table on o.Id equals oi.OrderId
                    join p in _productRepository.Table on oi.ProductId equals p.Id
                    where p.VendorId == vendorId
                    select o;

            query = query.Distinct();
        }

        if (productId > 0)
        {
            query = from o in query
                    join oi in _orderItemRepository.Table on o.Id equals oi.OrderId
                    where oi.ProductId == productId
                    select o;

            query = query.Distinct();
        }

        if (warehouseId > 0)
        {
            var manageStockInventoryMethodId = (int)ManageInventoryMethod.ManageStock;

            query = from o in query
                    join oi in _orderItemRepository.Table on o.Id equals oi.OrderId
                    join p in _productRepository.Table on oi.ProductId equals p.Id
                    join pwi in _productWarehouseInventoryRepository.Table on p.Id equals pwi.ProductId
                    where
                        //"Use multiple warehouses" enabled
                        //we search in each warehouse
                        (p.ManageInventoryMethodId == manageStockInventoryMethodId && p.UseMultipleWarehouses && pwi.WarehouseId == warehouseId) ||
                        //"Use multiple warehouses" disabled
                        //we use standard "warehouse" property
                        ((p.ManageInventoryMethodId != manageStockInventoryMethodId || !p.UseMultipleWarehouses) && p.WarehouseId == warehouseId)
                    select o;

            query = query.Distinct();
        }

        query = from o in query
                join oba in _addressRepository.Table on o.BillingAddressId equals oba.Id
                where
                    (billingCountryId <= 0 || (oba.CountryId == billingCountryId)) &&
                    (string.IsNullOrEmpty(billingPhone) || (!string.IsNullOrEmpty(oba.PhoneNumber) && oba.PhoneNumber.Contains(billingPhone))) &&
                    (string.IsNullOrEmpty(billingEmail) || (!string.IsNullOrEmpty(oba.Email) && oba.Email.Contains(billingEmail))) &&
                    (string.IsNullOrEmpty(billingLastName) || (!string.IsNullOrEmpty(oba.LastName) && oba.LastName.Contains(billingLastName)))
                select o;

        if (!string.IsNullOrEmpty(paymentMethodSystemName))
            query = query.Where(o => o.PaymentMethodSystemName == paymentMethodSystemName);

        if (osIds != null && osIds.Any())
            query = query.Where(o => osIds.Contains(o.OrderStatusId));

        if (psIds != null && psIds.Any())
            query = query.Where(o => psIds.Contains(o.PaymentStatusId));

        if (ssIds != null && ssIds.Any())
            query = query.Where(o => ssIds.Contains(o.ShippingStatusId));

        if (startTimeUtc.HasValue)
            query = query.Where(o => startTimeUtc.Value <= o.CreatedOnUtc);

        if (endTimeUtc.HasValue)
            query = query.Where(o => endTimeUtc.Value >= o.CreatedOnUtc);

        if (!string.IsNullOrEmpty(orderNotes))
        {
            query = from o in query
                    join n in _orderNoteRepository.Table on o.Id equals n.OrderId
                    where n.Note.Contains(orderNotes)
                    select o;

            query = query.Distinct();
        }

        var item = await (from oq in query
                          group oq by 1
                into result
                          select new
                          {
                              OrderCount = result.Count(),
                              OrderShippingExclTaxSum = result.Sum(o => o.OrderShippingExclTax),
                              OrderPaymentFeeExclTaxSum = result.Sum(o => o.PaymentMethodAdditionalFeeExclTax),
                              OrderTaxSum = result.Sum(o => o.OrderTax),
                              OrderTotalSum = result.Sum(o => o.OrderTotal),
                              OrederRefundedAmountSum = result.Sum(o => o.RefundedAmount),
                          }).Select(r => new OrderAverageReportLine
                          {
                              CountOrders = r.OrderCount,
                              SumShippingExclTax = r.OrderShippingExclTaxSum,
                              OrderPaymentFeeExclTaxSum = r.OrderPaymentFeeExclTaxSum,
                              SumTax = r.OrderTaxSum,
                              SumOrders = r.OrderTotalSum,
                              SumRefundedAmount = r.OrederRefundedAmountSum
                          })
            .FirstOrDefaultAsync();

        item ??= new OrderAverageReportLine
        {
            CountOrders = 0,
            SumShippingExclTax = decimal.Zero,
            OrderPaymentFeeExclTaxSum = decimal.Zero,
            SumTax = decimal.Zero,
            SumOrders = decimal.Zero
        };
        return item;
    }

    /// <summary>
    /// Get order average report
    /// </summary>
    /// <param name="storeId">Store identifier</param>
    /// <param name="os">Order status</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the result
    /// </returns>
    public virtual async Task<OrderAverageReportLineSummary> OrderAverageReportAsync(int storeId, OrderStatus os)
    {
        var item = new OrderAverageReportLineSummary
        {
            OrderStatus = os
        };
        var orderStatuses = new List<int> { (int)os };

        var nowDt = await _dateTimeHelper.ConvertToUserTimeAsync(DateTime.Now);
        var timeZone = await _dateTimeHelper.GetCurrentTimeZoneAsync();

        //today
        var t1 = new DateTime(nowDt.Year, nowDt.Month, nowDt.Day);
        DateTime? startTime1 = _dateTimeHelper.ConvertToUtcTime(t1, timeZone);
        var todayResult = await GetOrderAverageReportLineAsync(storeId,
            osIds: orderStatuses,
            startTimeUtc: startTime1);
        item.SumTodayOrders = todayResult.SumOrders;
        item.CountTodayOrders = todayResult.CountOrders;

        //week
        var fdow = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
        var today = new DateTime(nowDt.Year, nowDt.Month, nowDt.Day);
        var t2 = today.AddDays(-(today.DayOfWeek - fdow));
        DateTime? startTime2 = _dateTimeHelper.ConvertToUtcTime(t2, timeZone);
        var weekResult = await GetOrderAverageReportLineAsync(storeId,
            osIds: orderStatuses,
            startTimeUtc: startTime2);
        item.SumThisWeekOrders = weekResult.SumOrders;
        item.CountThisWeekOrders = weekResult.CountOrders;

        //month
        var t3 = new DateTime(nowDt.Year, nowDt.Month, 1);
        DateTime? startTime3 = _dateTimeHelper.ConvertToUtcTime(t3, timeZone);
        var monthResult = await GetOrderAverageReportLineAsync(storeId,
            osIds: orderStatuses,
            startTimeUtc: startTime3);
        item.SumThisMonthOrders = monthResult.SumOrders;
        item.CountThisMonthOrders = monthResult.CountOrders;

        //year
        var t4 = new DateTime(nowDt.Year, 1, 1);
        DateTime? startTime4 = _dateTimeHelper.ConvertToUtcTime(t4, timeZone);
        var yearResult = await GetOrderAverageReportLineAsync(storeId,
            osIds: orderStatuses,
            startTimeUtc: startTime4);
        item.SumThisYearOrders = yearResult.SumOrders;
        item.CountThisYearOrders = yearResult.CountOrders;

        //all time
        var allTimeResult = await GetOrderAverageReportLineAsync(storeId, osIds: orderStatuses);
        item.SumAllTimeOrders = allTimeResult.SumOrders;
        item.CountAllTimeOrders = allTimeResult.CountOrders;

        return item;
    }

    /// <summary>
    /// Get sales summary report
    /// </summary>
    /// <param name="categoryId">Category identifier; 0 to load all records</param>
    /// <param name="productId">Product identifier; 0 to load all records</param>
    /// <param name="manufacturerId">Manufacturer identifier; 0 to load all records</param>
    /// <param name="storeId">Store identifier (orders placed in a specific store); 0 to load all records</param>
    /// <param name="vendorId">Vendor identifier; 0 to load all records</param>
    /// <param name="createdFromUtc">Order created date from (UTC); null to load all records</param>
    /// <param name="createdToUtc">Order created date to (UTC); null to load all records</param>
    /// <param name="osIds">Order status identifiers; null to load all orders</param>
    /// <param name="psIds">Payment status identifiers; null to load all orders</param>
    /// <param name="billingCountryId">Billing country identifier; 0 to load all records</param>
    /// <param name="groupBy">0 - group by day, 1 - group by week, 2 - group by total month</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the result
    /// </returns>
    public virtual async Task<IPagedList<SalesSummaryReportLine>> SalesSummaryReportAsync(
        int categoryId = 0,
        int productId = 0,
        int manufacturerId = 0,
        int storeId = 0,
        int vendorId = 0,
        DateTime? createdFromUtc = null,
        DateTime? createdToUtc = null,
        List<int> osIds = null,
        List<int> psIds = null,
        int billingCountryId = 0,
        GroupByOptions groupBy = GroupByOptions.Day,
        int pageIndex = 0,
        int pageSize = int.MaxValue)
    {
        //var orderItems = SearchOrderItems(categoryId, manufacturerId, storeId, vendorId, createdFromUtc, createdToUtc, os, ps, null, billingCountryId);

        var query = _orderRepository.Table;
        query = query.Where(o => !o.Deleted);

        //filter by date
        if (createdFromUtc.HasValue)
            query = query.Where(o => createdFromUtc.Value <= o.CreatedOnUtc);

        if (createdToUtc.HasValue)
            query = query.Where(o => createdToUtc.Value >= o.CreatedOnUtc);

        //filter by order status
        if (osIds != null && osIds.Any())
            query = query.Where(o => osIds.Contains(o.OrderStatusId));

        //filter by payment status
        if (psIds != null && psIds.Any())
            query = query.Where(o => psIds.Contains(o.PaymentStatusId));

        //filter by category
        if (categoryId > 0)
        {
            query = from o in query
                    join oi in _orderItemRepository.Table on o.Id equals oi.OrderId
                    join p in _productRepository.Table on oi.ProductId equals p.Id
                    join pc in _productCategoryRepository.Table on p.Id equals pc.ProductId
                    where pc.CategoryId == categoryId
                    select o;

            query = query.Distinct();
        }

        //filter by manufacturer
        if (manufacturerId > 0)
        {
            query = from o in query
                    join oi in _orderItemRepository.Table on o.Id equals oi.OrderId
                    join p in _productRepository.Table on oi.ProductId equals p.Id
                    join pm in _productManufacturerRepository.Table on p.Id equals pm.ProductId
                    where pm.ManufacturerId == manufacturerId
                    select o;

            query = query.Distinct();
        }

        //filter by country
        if (billingCountryId > 0)
        {
            query = from o in query
                    join oba in _addressRepository.Table on o.BillingAddressId equals oba.Id
                    where
                        billingCountryId <= 0 || oba.CountryId == billingCountryId
                    select o;

            query = query.Distinct();
        }

        //filter by product
        if (productId > 0)
        {
            query = from o in query
                    join oi in _orderItemRepository.Table on o.Id equals oi.OrderId
                    where oi.ProductId == productId
                    select o;

            query = query.Distinct();
        }

        //filter by store
        if (storeId > 0)
            query = query.Where(o => o.StoreId == storeId);

        if (vendorId > 0)
        {
            query = from o in query
                    join oi in _orderItemRepository.Table on o.Id equals oi.OrderId
                    join p in _productRepository.Table on oi.ProductId equals p.Id
                    where p.VendorId == vendorId
                    select o;

            query = query.Distinct();
        }

        var primaryStoreCurrency = await _currencyService.GetCurrencyByIdAsync(_currencySettings.PrimaryStoreCurrencyId);
        var userTimeZone = await _dateTimeHelper.GetCurrentTimeZoneAsync();
        var utcOffsetInMinutes = userTimeZone.BaseUtcOffset.TotalMinutes;

        var items = groupBy switch
        {
            GroupByOptions.Day => from oq in query
                                  group oq by oq.CreatedOnUtc.AddMinutes(utcOffsetInMinutes).Date into result
                                  let orderItems = _orderItemRepository.Table.Where(oi => result.Any(x => x.Id == oi.OrderId))
                                  select new
                                  {
                                      SummaryDate = result.Key,
                                      OrderSummaryType = groupBy,
                                      OrderCount = result.Count(),
                                      OrderShippingExclTaxSum = result.Sum(o => o.OrderShippingExclTax),
                                      OrderPaymentFeeExclTaxSum = result.Sum(o => o.PaymentMethodAdditionalFeeExclTax),
                                      OrderTaxSum = result.Sum(o => o.OrderTax),
                                      OrderTotalSum = result.Sum(o => o.OrderTotal),
                                      OrderRefundedAmountSum = result.Sum(o => o.RefundedAmount),
                                      OrderTotalCost = orderItems.Sum(oi => (decimal?)oi.OriginalProductCost * oi.Quantity)
                                  },
            GroupByOptions.Week => from oq in query
                                   group oq by oq.CreatedOnUtc.AddMinutes(utcOffsetInMinutes).AddDays(-(int)oq.CreatedOnUtc.AddMinutes(utcOffsetInMinutes).DayOfWeek).Date into result
                                   let orderItems = _orderItemRepository.Table.Where(oi => result.Any(x => x.Id == oi.OrderId))
                                   select new
                                   {
                                       SummaryDate = result.Key,
                                       OrderSummaryType = groupBy,
                                       OrderCount = result.Count(),
                                       OrderShippingExclTaxSum = result.Sum(o => o.OrderShippingExclTax),
                                       OrderPaymentFeeExclTaxSum = result.Sum(o => o.PaymentMethodAdditionalFeeExclTax),
                                       OrderTaxSum = result.Sum(o => o.OrderTax),
                                       OrderTotalSum = result.Sum(o => o.OrderTotal),
                                       OrderRefundedAmountSum = result.Sum(o => o.RefundedAmount),
                                       OrderTotalCost = orderItems.Sum(oi => (decimal?)oi.OriginalProductCost * oi.Quantity)
                                   },
            GroupByOptions.Month => from oq in query
                                    group oq by oq.CreatedOnUtc.AddMinutes(utcOffsetInMinutes).AddDays(1 - oq.CreatedOnUtc.AddMinutes(utcOffsetInMinutes).Day).Date into result
                                    let orderItems = _orderItemRepository.Table.Where(oi => result.Any(x => x.Id == oi.OrderId))
                                    select new
                                    {
                                        SummaryDate = result.Key,
                                        OrderSummaryType = groupBy,
                                        OrderCount = result.Count(),
                                        OrderShippingExclTaxSum = result.Sum(o => o.OrderShippingExclTax),
                                        OrderPaymentFeeExclTaxSum = result.Sum(o => o.PaymentMethodAdditionalFeeExclTax),
                                        OrderTaxSum = result.Sum(o => o.OrderTax),
                                        OrderTotalSum = result.Sum(o => o.OrderTotal),
                                        OrderRefundedAmountSum = result.Sum(o => o.RefundedAmount),
                                        OrderTotalCost = orderItems.Sum(oi => (decimal?)oi.OriginalProductCost * oi.Quantity)
                                    },
            _ => throw new ArgumentException("Wrong groupBy parameter", nameof(groupBy)),
        };

        var ssReport =
            from orderItem in items
            orderby orderItem.SummaryDate descending
            select new SalesSummaryReportLine
            {
                SummaryDate = orderItem.SummaryDate,
                SummaryType = (int)orderItem.OrderSummaryType,
                NumberOfOrders = orderItem.OrderCount,
                Profit = orderItem.OrderTotalSum
                         - orderItem.OrderShippingExclTaxSum
                         - orderItem.OrderPaymentFeeExclTaxSum
                         - orderItem.OrderTaxSum
                         - orderItem.OrderRefundedAmountSum
                         - Convert.ToDecimal(orderItem.OrderTotalCost),
                Shipping = orderItem.OrderShippingExclTaxSum.ToString(CultureInfo.CurrentCulture),
                Tax = orderItem.OrderTaxSum.ToString(CultureInfo.CurrentCulture),
                OrderTotal = orderItem.OrderTotalSum.ToString(CultureInfo.CurrentCulture)
            };

        var report = await ssReport.ToPagedListAsync(pageIndex, pageSize);

        static string formatSummary(DateTime dt, GroupByOptions groupByOption)
        {
            const string dayFormat = "MMM dd, yyyy";

            return groupByOption switch
            {
                GroupByOptions.Week => $"{dt.ToString(dayFormat)} - {dt.AddDays(6).ToString(dayFormat)}",
                GroupByOptions.Day => dt.ToString(dayFormat),
                GroupByOptions.Month => dt.ToString("MMMM, yyyy"),
                _ => ""
            };
        }

        foreach (var reportLine in report)
        {
            reportLine.Summary = formatSummary(reportLine.SummaryDate, groupBy);

            reportLine.ProfitStr = await _priceFormatter.FormatPriceAsync(reportLine.Profit, true, false);
            reportLine.Shipping = await _priceFormatter
                .FormatShippingPriceAsync(Convert.ToDecimal(reportLine.Shipping), true, primaryStoreCurrency, (await _workContext.GetWorkingLanguageAsync()).Id, false);
            reportLine.Tax = await _priceFormatter.FormatPriceAsync(Convert.ToDecimal(reportLine.Tax), true, false);
            reportLine.OrderTotal = await _priceFormatter.FormatPriceAsync(Convert.ToDecimal(reportLine.OrderTotal), true, false);
        }

        return report;
    }

    /// <summary>
    /// Get best sellers report
    /// </summary>
    /// <param name="storeId">Store identifier (orders placed in a specific store); 0 to load all records</param>
    /// <param name="vendorId">Vendor identifier; 0 to load all records</param>
    /// <param name="categoryId">Category identifier; 0 to load all records</param>
    /// <param name="manufacturerId">Manufacturer identifier; 0 to load all records</param>
    /// <param name="createdFromUtc">Order created date from (UTC); null to load all records</param>
    /// <param name="createdToUtc">Order created date to (UTC); null to load all records</param>
    /// <param name="os">Order status; null to load all records</param>
    /// <param name="ps">Order payment status; null to load all records</param>
    /// <param name="ss">Shipping status; null to load all records</param>
    /// <param name="billingCountryId">Billing country identifier; 0 to load all records</param>
    /// <param name="orderBy">1 - order by quantity, 2 - order by total amount</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="showHidden">A value indicating whether to show hidden records</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the result
    /// </returns>
    public virtual async Task<IPagedList<BestsellersReportLine>> BestSellersReportAsync(
        int categoryId = 0,
        int manufacturerId = 0,
        int storeId = 0,
        int vendorId = 0,
        DateTime? createdFromUtc = null,
        DateTime? createdToUtc = null,
        OrderStatus? os = null,
        PaymentStatus? ps = null,
        ShippingStatus? ss = null,
        int billingCountryId = 0,
        OrderByEnum orderBy = OrderByEnum.OrderByQuantity,
        int pageIndex = 0,
        int pageSize = int.MaxValue,
        bool showHidden = false)
    {
        var bestSellers = SearchOrderItems(categoryId, manufacturerId, storeId, vendorId, createdFromUtc, createdToUtc, os, ps, ss, billingCountryId, showHidden);

        var bsReport =
            //group by products
            from orderItem in bestSellers
            group orderItem by orderItem.ProductId into g
            select new BestsellersReportLine
            {
                ProductId = g.Key,
                TotalAmount = g.Sum(x => x.PriceExclTax),
                TotalQuantity = g.Sum(x => x.Quantity)
            };

        bsReport = from item in bsReport
                   join p in _productRepository.Table on item.ProductId equals p.Id
                   join v in _vendorRepository.Table on p.VendorId equals v.Id
                   select new BestsellersReportLine
                   {
                       ProductId = item.ProductId,
                       TotalAmount = item.TotalAmount,
                       TotalQuantity = item.TotalQuantity,
                       ProductName = p.Name,
                       VendorName = v.Name,
                   };

        bsReport = orderBy switch
        {
            OrderByEnum.OrderByQuantity => bsReport.OrderByDescending(x => x.TotalQuantity),
            OrderByEnum.OrderByTotalAmount => bsReport.OrderByDescending(x => x.TotalAmount),
            _ => throw new ArgumentException("Wrong orderBy parameter", nameof(orderBy)),
        };
        var bsReportWithRank = bsReport
        .Select((item, index) => new BestsellersReportLine
        {
            ProductId = item.ProductId,
            ProductName = item.ProductName,
            VendorName = item.VendorName,
            TotalAmount = item.TotalAmount,
            TotalQuantity = item.TotalQuantity,
            Rank = index + 1 // Adding rank directly
        });
        var result = await bsReportWithRank.ToPagedListAsync(pageIndex, pageSize);

        return result;
    }

    /// <summary>
    /// Get best sellers total amount
    /// </summary>
    /// <param name="storeId">Store identifier (orders placed in a specific store); 0 to load all records</param>
    /// <param name="vendorId">Vendor identifier; 0 to load all records</param>
    /// <param name="categoryId">Category identifier; 0 to load all records</param>
    /// <param name="manufacturerId">Manufacturer identifier; 0 to load all records</param>
    /// <param name="createdFromUtc">Order created date from (UTC); null to load all records</param>
    /// <param name="createdToUtc">Order created date to (UTC); null to load all records</param>
    /// <param name="os">Order status; null to load all records</param>
    /// <param name="ps">Order payment status; null to load all records</param>
    /// <param name="ss">Shipping status; null to load all records</param>
    /// <param name="billingCountryId">Billing country identifier; 0 to load all records</param>
    /// <param name="showHidden">A value indicating whether to show hidden records</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the result
    /// </returns>
    public virtual async Task<decimal> BestSellersReportTotalAmountAsync(
        int categoryId = 0,
        int manufacturerId = 0,
        int storeId = 0,
        int vendorId = 0,
        DateTime? createdFromUtc = null,
        DateTime? createdToUtc = null,
        OrderStatus? os = null,
        PaymentStatus? ps = null,
        ShippingStatus? ss = null,
        int billingCountryId = 0,
        bool showHidden = false)
    {
        return await SearchOrderItems(categoryId, manufacturerId, storeId, vendorId, createdFromUtc, createdToUtc, os, ps, ss, billingCountryId, showHidden: showHidden)
            .SumAsync(bestseller => bestseller.PriceExclTax);
    }

    /// <summary>
    /// Gets a list of products (identifiers) purchased by other customers who purchased a specified product
    /// </summary>
    /// <param name="storeId">Store identifier</param>
    /// <param name="productId">Product identifier</param>
    /// <param name="recordsToReturn">Records to return</param>
    /// <param name="visibleIndividuallyOnly">A values indicating whether to load only products marked as "visible individually"; "false" to load all records; "true" to load "visible individually" only</param>
    /// <param name="showHidden">A value indicating whether to show hidden records</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the products
    /// </returns>
    public virtual async Task<int[]> GetAlsoPurchasedProductsIdsAsync(int storeId, int productId,
        int recordsToReturn = 5, bool visibleIndividuallyOnly = true, bool showHidden = false)
    {
        if (productId == 0)
            throw new ArgumentException("Product ID is not specified");

        //this inner query should retrieve all orders that contains a specified product ID
        var query1 = from orderItem in _orderItemRepository.Table
                     where orderItem.ProductId == productId
                     select orderItem.OrderId;

        var query2 = from orderItem in _orderItemRepository.Table
                     join p in _productRepository.Table on orderItem.ProductId equals p.Id
                     join o in _orderRepository.Table on orderItem.OrderId equals o.Id
                     where query1.Contains(orderItem.OrderId) &&
                           p.Id != productId &&
                           (showHidden || p.Published) &&
                           !o.Deleted &&
                           (storeId == 0 || o.StoreId == storeId) &&
                           !p.Deleted &&
                           (!visibleIndividuallyOnly || p.VisibleIndividually)
                     select new { orderItem, p };

        var query3 = from orderItem_p in query2
                     group orderItem_p by orderItem_p.p.Id into g
                     select new
                     {
                         ProductId = g.Key,
                         ProductsPurchased = g.Sum(x => x.orderItem.Quantity)
                     };
        query3 = query3.OrderByDescending(x => x.ProductsPurchased);

        if (recordsToReturn > 0)
            query3 = query3.Take(recordsToReturn);

        var report = await query3.ToListAsync();

        var ids = new List<int>();
        foreach (var reportLine in report)
            ids.Add(reportLine.ProductId);

        return ids.ToArray();
    }

    /// <summary>
    /// Gets a list of products that were never sold
    /// </summary>
    /// <param name="vendorId">Vendor identifier (filter products by a specific vendor); 0 to load all records</param>
    /// <param name="storeId">Store identifier (filter products by a specific store); 0 to load all records</param>
    /// <param name="categoryId">Category identifier; 0 to load all records</param>
    /// <param name="manufacturerId">Manufacturer identifier; 0 to load all records</param>
    /// <param name="createdFromUtc">Order created date from (UTC); null to load all records</param>
    /// <param name="createdToUtc">Order created date to (UTC); null to load all records</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="showHidden">A value indicating whether to show hidden records</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the products
    /// </returns>
    public virtual async Task<IPagedList<Product>> ProductsNeverSoldAsync(int vendorId = 0, int storeId = 0,
        int categoryId = 0, int manufacturerId = 0,
        DateTime? createdFromUtc = null, DateTime? createdToUtc = null,
        int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
    {
        var simpleProductTypeId = (int)ProductType.SimpleProduct;

        var availableProductsQuery =
            from oi in _orderItemRepository.Table
            join o in _orderRepository.Table on oi.OrderId equals o.Id
            where (!createdFromUtc.HasValue || createdFromUtc.Value <= o.CreatedOnUtc) &&
                  (!createdToUtc.HasValue || createdToUtc.Value >= o.CreatedOnUtc) &&
                  !o.Deleted
            select new { oi.ProductId };

        var query =
            from p in _productRepository.Table
            join vn in _vendorRepository.Table on p.VendorId equals vn.Id
            join oi in availableProductsQuery on p.Id equals oi.ProductId
                into p_oi
            from oi in p_oi.DefaultIfEmpty()
            where oi == null &&
                  p.ProductTypeId == simpleProductTypeId &&
                  !p.Deleted &&
                  (vendorId == 0 || p.VendorId == vendorId) &&
                  (showHidden || p.Published)
            select p;

        if (categoryId > 0)
        {
            query = from p in query
                    join pc in _productCategoryRepository.Table on p.Id equals pc.ProductId
                        into p_pc
                    from pc in p_pc.DefaultIfEmpty()
                    where pc.CategoryId == categoryId
                    select p;
        }

        if (manufacturerId > 0)
        {
            query = from p in query
                    join pm in _productManufacturerRepository.Table on p.Id equals pm.ProductId
                        into p_pm
                    from pm in p_pm.DefaultIfEmpty()
                    where pm.ManufacturerId == manufacturerId
                    select p;
        }

        //apply store mapping constraints
        query = await _storeMappingService.ApplyStoreMapping(query, storeId);

        query = query.OrderBy(p => p.Name);

        var products = await query.ToPagedListAsync(pageIndex, pageSize);
        return products;
    }

    /// <summary>
    /// Get profit report
    /// </summary>
    /// <param name="storeId">Store identifier; pass 0 to ignore this parameter</param>
    /// <param name="vendorId">Vendor identifier; pass 0 to ignore this parameter</param>
    /// <param name="productId">Product identifier which was purchased in an order; 0 to load all orders</param>
    /// <param name="warehouseId">Warehouse identifier; pass 0 to ignore this parameter</param>
    /// <param name="orderId">Order identifier; pass 0 to ignore this parameter</param>
    /// <param name="billingCountryId">Billing country identifier; 0 to load all orders</param>
    /// <param name="paymentMethodSystemName">Payment method system name; null to load all records</param>
    /// <param name="startTimeUtc">Start date</param>
    /// <param name="endTimeUtc">End date</param>
    /// <param name="osIds">Order status identifiers; null to load all records</param>
    /// <param name="psIds">Payment status identifiers; null to load all records</param>
    /// <param name="ssIds">Shipping status identifiers; null to load all records</param>
    /// <param name="billingPhone">Billing phone. Leave empty to load all records.</param>
    /// <param name="billingEmail">Billing email. Leave empty to load all records.</param>
    /// <param name="billingLastName">Billing last name. Leave empty to load all records.</param>
    /// <param name="orderNotes">Search in order notes. Leave empty to load all records.</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the result
    /// </returns>
    public virtual async Task<decimal> ProfitReportAsync(int storeId = 0, int vendorId = 0, int productId = 0,
        int warehouseId = 0, int billingCountryId = 0, int orderId = 0, string paymentMethodSystemName = null,
        List<int> osIds = null, List<int> psIds = null, List<int> ssIds = null,
        DateTime? startTimeUtc = null, DateTime? endTimeUtc = null,
        string billingPhone = null, string billingEmail = null, string billingLastName = "", string orderNotes = null)
    {
        var dontSearchPhone = string.IsNullOrEmpty(billingPhone);
        var dontSearchEmail = string.IsNullOrEmpty(billingEmail);
        var dontSearchLastName = string.IsNullOrEmpty(billingLastName);
        var dontSearchOrderNotes = string.IsNullOrEmpty(orderNotes);
        var dontSearchPaymentMethods = string.IsNullOrEmpty(paymentMethodSystemName);

        var orders = _orderRepository.Table;
        if (osIds != null && osIds.Any())
            orders = orders.Where(o => osIds.Contains(o.OrderStatusId));
        if (psIds != null && psIds.Any())
            orders = orders.Where(o => psIds.Contains(o.PaymentStatusId));
        if (ssIds != null && ssIds.Any())
            orders = orders.Where(o => ssIds.Contains(o.ShippingStatusId));

        var manageStockInventoryMethodId = (int)ManageInventoryMethod.ManageStock;

        var query = from orderItem in _orderItemRepository.Table
                    join o in orders on orderItem.OrderId equals o.Id
                    join p in _productRepository.Table on orderItem.ProductId equals p.Id
                    join oba in _addressRepository.Table on o.BillingAddressId equals oba.Id
                    where (storeId == 0 || storeId == o.StoreId) &&
                          (orderId == 0 || orderId == o.Id) &&
                          (billingCountryId == 0 || (oba.CountryId == billingCountryId)) &&
                          (dontSearchPaymentMethods || paymentMethodSystemName == o.PaymentMethodSystemName) &&
                          (!startTimeUtc.HasValue || startTimeUtc.Value <= o.CreatedOnUtc) &&
                          (!endTimeUtc.HasValue || endTimeUtc.Value >= o.CreatedOnUtc) &&
                          !o.Deleted &&
                          (vendorId == 0 || p.VendorId == vendorId) &&
                          (productId == 0 || orderItem.ProductId == productId) &&
                          (warehouseId == 0 ||
                           //"Use multiple warehouses" enabled
                           //we search in each warehouse
                           p.ManageInventoryMethodId == manageStockInventoryMethodId &&
                           p.UseMultipleWarehouses &&
                           _productWarehouseInventoryRepository.Table.Any(pwi =>
                               pwi.ProductId == orderItem.ProductId && pwi.WarehouseId == warehouseId)
                           ||
                           //"Use multiple warehouses" disabled
                           //we use standard "warehouse" property
                           (p.ManageInventoryMethodId != manageStockInventoryMethodId ||
                            !p.UseMultipleWarehouses) &&
                           p.WarehouseId == warehouseId) &&
                          //we do not ignore deleted products when calculating order reports
                          //(!p.Deleted)
                          (dontSearchPhone || (!string.IsNullOrEmpty(oba.PhoneNumber) &&
                                               oba.PhoneNumber.Contains(billingPhone))) &&
                          (dontSearchEmail || (!string.IsNullOrEmpty(oba.Email) && oba.Email.Contains(billingEmail))) &&
                          (dontSearchLastName ||
                           (!string.IsNullOrEmpty(oba.LastName) && oba.LastName.Contains(billingLastName))) &&
                          (dontSearchOrderNotes || _orderNoteRepository.Table.Any(oNote =>
                              oNote.OrderId == o.Id && oNote.Note.Contains(orderNotes)))
                    select orderItem;

        var productCost = Convert.ToDecimal(await query.SumAsync(orderItem => (decimal?)orderItem.OriginalProductCost * orderItem.Quantity));

        var reportSummary = await GetOrderAverageReportLineAsync(
            storeId,
            vendorId,
            productId,
            warehouseId,
            billingCountryId,
            orderId,
            paymentMethodSystemName,
            osIds,
            psIds,
            ssIds,
            startTimeUtc,
            endTimeUtc,
            billingPhone,
            billingEmail,
            billingLastName,
            orderNotes);

        var profit = reportSummary.SumOrders
                     - reportSummary.SumShippingExclTax
                     - reportSummary.OrderPaymentFeeExclTaxSum
                     - reportSummary.SumTax
                     - reportSummary.SumRefundedAmount
                     - productCost;
        return profit;
    }

    public async Task<IPagedList<CategoryOrderReportLine>> GetBestSellingCategoriesAsync(DateTime? startDate, DateTime? endDate, int pageIndex = 0, int pageSize = 10)
    {
        var categoryOrderData = (from orderItem in _orderItemRepository.Table
                                 join order in _orderRepository.Table on orderItem.OrderId equals order.Id
                                 join product in _productRepository.Table on orderItem.ProductId equals product.Id
                                 join productCategory in _productCategoryRepository.Table on product.Id equals productCategory.ProductId
                                 join category in _categoryRepository.Table on productCategory.CategoryId equals category.Id
                                 where (!startDate.HasValue || order.CreatedOnUtc >= startDate.Value)
                                    && (!endDate.HasValue || order.CreatedOnUtc <= endDate.Value)
                                 group orderItem by category into grouped
                                 orderby grouped.Count() descending
                                 select new CategoryOrderReportLine
                                 {
                                     CategoryId = grouped.Key.Id,
                                     CategoryName = grouped.Key.Name,
                                     NumberOfOrders = grouped.Count(),
                                     Rank = 0 // Placeholder, will be calculated later
                                 });

        var rankedCategoryOrderData = categoryOrderData.Select((x, index) => new CategoryOrderReportLine
        {
            CategoryId = x.CategoryId,
            CategoryName = x.CategoryName,
            NumberOfOrders = x.NumberOfOrders,
            Rank = index + 1
        });


        var result = await rankedCategoryOrderData.ToPagedListAsync(pageIndex, pageSize);

        return result;

        //  return await rankedCategoryOrderData.ToListAsync();
    }

    public async Task<IPagedList<VendorOrderReportLine>> GetBestSellingVendorsAsync(DateTime? startDate, DateTime? endDate, int pageIndex = 0, int pageSize = 10)
    {
        var query = from orderItem in _orderItemRepository.Table
                    join order in _orderRepository.Table on orderItem.OrderId equals order.Id
                    join product in _productRepository.Table on orderItem.ProductId equals product.Id
                    join vendor in _vendorRepository.Table on product.VendorId equals vendor.Id
                    where (!startDate.HasValue || order.CreatedOnUtc >= startDate.Value)
                       && (!endDate.HasValue || order.CreatedOnUtc <= endDate.Value)
                    group orderItem by vendor into grouped
                    orderby grouped.Count() descending
                    select new VendorOrderReportLine
                    {
                        VendorId = grouped.Key.Id,
                        VendorName = grouped.Key.Name,
                        NumberOfOrders = grouped.Count()
                    };



        var rankedRankedOrderData = query.Select((x, index) => new VendorOrderReportLine
        {
            VendorId = x.VendorId,
            VendorName = x.VendorName,
            NumberOfOrders = x.NumberOfOrders,
            Rank = index + 1
        });
        var result = await rankedRankedOrderData.ToPagedListAsync(pageIndex, pageSize);

        return result;
    }


    public async Task<IPagedList<PeekHourReportLine>> GetPeekHoursAsync(DateTime? startDate, DateTime? endDate, int pageIndex = 0, int pageSize = 10)

    {
        var orders =  await (from order in _orderRepository.Table
                            join addr in _addressRepository.Table on order.ShippingAddressId equals addr.Id
                            join country in _countryRepository.Table on addr.CountryId equals country.Id
                            group order by new { addr.CountryId, Hour = order.CreatedOnUtc.Hour, country.Name } into g
                            select new
                            {
                                CountryId = g.Key.CountryId,
                                Hour = g.Key.Hour,
                                NumberOfOrders = g.Count(),
                                CountryName = g.Key.Name
                            }).OrderByDescending(x => x.NumberOfOrders)
                            .ToListAsync();
    // 

        // Find the peak hour for each country
        var peakHourByCountry = orders
            .GroupBy(o => o.CountryId)
            .Select(g => g.OrderByDescending(x => x.NumberOfOrders).Take(3))
            .ToList();

        var rankedRankedOrderData = peakHourByCountry.Select((x, index) => new PeekHourReportLine
        {

            CountryId = x.FirstOrDefault().CountryId.Value,
            CountryName = x.FirstOrDefault().CountryName,
            PeakHour = x.Select(h=>h.Hour).ToList(),
            NumberOfOrders = x.Select(h => h.NumberOfOrders).ToList(),
            Rank = index + 1
        });
        var result = await rankedRankedOrderData.AsQueryable().ToPagedListAsync(pageIndex, pageSize);

        return result;

    }

    public async Task<IPagedList<SalesItemsReportLine>> SalesItemsReportAsync(DateTime? startDate, DateTime? endDate, int pageIndex = 0, int pageSize = 10)
    {
        var salesReport = from orderItem in _orderItemRepository.Table
                          join order in _orderRepository.Table on orderItem.OrderId equals order.Id
                          join product in _productRepository.Table on orderItem.ProductId equals product.Id
                          join vendor in _vendorRepository.Table on product.VendorId equals vendor.Id
                          where (!startDate.HasValue || order.CreatedOnUtc >= startDate.Value)
                             && (!endDate.HasValue || order.CreatedOnUtc <= endDate.Value)
                          orderby order.CreatedOnUtc descending // Order by order date, latest to oldest
                          select new SalesItemsReportLine
                          {
                              OrderId = order.Id,
                              OrderDate = order.CreatedOnUtc,
                              ProductName = product.Name,
                              VendorName = vendor.Name,
                              Quantity = orderItem.Quantity,
                              TotalAmount = orderItem.PriceExclTax * orderItem.Quantity
                          };

        var rankedRankedOrderData = salesReport.Select((x, index) => new SalesItemsReportLine
        {
            OrderDate = x.OrderDate,
            OrderId = x.OrderId,
            ProductName = x.ProductName,
            Quantity = x.Quantity,
            VendorName = x.VendorName,
            TotalAmount = x.TotalAmount,  // await _priceFormatter.FormatPriceAsync(Convert.ToDecimal(x.TotalAmount), true, false),
            Rank = index + 1
        });
        var result = await rankedRankedOrderData.ToPagedListAsync(pageIndex, pageSize);

        return result;

    }

    public async Task<EarningsReportModel> GetEarningsReportAsync(DateTime? startDate, DateTime? endDate)
    {
        var orders = _orderRepository.Table;

        if (startDate.HasValue)
            orders = orders.Where(o => o.CreatedOnUtc >= startDate.Value);

        if (endDate.HasValue)
            orders = orders.Where(o => o.CreatedOnUtc <= endDate.Value);

        // Total number of orders
        var totalOrders = await orders.CountAsync();

        // Total value of orders
        var totalOrderValue = await orders.SumAsync(o => o.OrderTotal);

        // Total order cost (using join)
        var totalOrderCost = await (from o in orders
                                    join oi in _orderItemRepository.Table on o.Id equals oi.OrderId
                                    select new
                                    {
                                        Cost = oi.OriginalProductCost * oi.Quantity
                                    })
                                   .SumAsync(x => x.Cost);

        // Total product prices (excluding tax)
        var totalProductsPrice = await (from o in orders
                                        join oi in _orderItemRepository.Table on o.Id equals oi.OrderId
                                        select new
                                        {
                                            Price = oi.PriceExclTax * oi.Quantity
                                        })
                                       .SumAsync(x => x.Price);

        // Total number of products sold
        var totalProductsSold = await (from o in orders
                                       join oi in _orderItemRepository.Table on o.Id equals oi.OrderId
                                       select oi)
                                       .CountAsync();

        // Total shipping fees
        var totalShippingFees = await orders.SumAsync(o => o.OrderShippingInclTax);

        // Total taxes
        var totalTaxes = await orders.SumAsync(o => o.OrderTax);

        // Total number of coupons used
        var totalCouponsUsed = await (from o in orders
                                      join oi in _orderItemRepository.Table on o.Id equals oi.OrderId
                                      join p in _productRepository.Table on oi.ProductId equals p.Id
                                      join dm in _discountProductMappingRepository.Table on p.Id equals dm.EntityId
                                      join d in _discountsRepository.Table on dm.DiscountId equals  d.Id
                                      where(!string.IsNullOrEmpty( d.CouponCode) && d.RequiresCouponCode == true)
                                      select oi)
                                       .CountAsync();

        // Total coupon value
        var totalCouponsValue = await (from o in orders
                                       join oi in _orderItemRepository.Table on o.Id equals oi.OrderId
                                       join p in _productRepository.Table on oi.ProductId equals p.Id
                                       join dm in _discountProductMappingRepository.Table on p.Id equals dm.EntityId
                                       join d in _discountsRepository.Table on dm.DiscountId equals d.Id
                                       where (!string.IsNullOrEmpty(d.CouponCode) && d.RequiresCouponCode == true)
                                       select oi)
                                       .SumAsync(o => o.DiscountAmountExclTax);

        // Total number of returned products
        var returnRequests = _returnRequestRepository.Table;
        if (startDate.HasValue)
            returnRequests = returnRequests.Where(r => r.CreatedOnUtc >= startDate.Value);
        if (endDate.HasValue)
            returnRequests = returnRequests.Where(r => r.CreatedOnUtc <= endDate.Value);

        var totalReturnedProducts = await (from rr in returnRequests
                                           join rri in _orderItemRepository.Table on rr.OrderItemId equals rri.Id
                                           select rri)
                                          .CountAsync();

        // Total value of returns (using join)
        var totalReturnsValue = await (from rr in returnRequests
                                       join oi in _orderItemRepository.Table on rr.OrderItemId equals oi.Id
                                       select new
                                       {
                                           ReturnValue = oi.PriceInclTax * rr.Quantity
                                       })
                                      .SumAsync(x => x.ReturnValue);

        return new EarningsReportModel
        {
            TotalOrders = totalOrders,
            TotalOrderValue = totalOrderValue,
            TotalOrderCost = totalOrderCost,
            TotalProductsPrice = totalProductsPrice,
            TotalProductsSold = totalProductsSold,
            TotalShippingFees = totalShippingFees,
            TotalTaxes = totalTaxes,
            TotalCouponsUsed = totalCouponsUsed,
            TotalCouponsValue = totalCouponsValue,
            TotalReturnedProducts = totalReturnedProducts,
            TotalReturnsValue = totalReturnsValue
        };
    }

    public async Task<IPagedList<ReturnedItemsPerVendorReportLine>> GetReturnedItemsPerVendorReportAsync(DateTime? startDate, DateTime? endDate,int? vendor, int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var returnRequests = _returnRequestRepository.Table;

        if (startDate.HasValue)
            returnRequests = returnRequests.Where(r => r.CreatedOnUtc >= startDate.Value);

        if (endDate.HasValue)
            returnRequests = returnRequests.Where(r => r.CreatedOnUtc <= endDate.Value);

      

        var report = await (from rr in returnRequests
                            join oi in _orderItemRepository.Table on rr.OrderItemId equals oi.Id
                            join p in _productRepository.Table on oi.ProductId equals p.Id
                            join v in _vendorRepository.Table on p.VendorId equals v.Id
                            where (vendor.HasValue & vendor != 0) ? v.Id == vendor.Value:true
                            group new { rr, oi, v } by v.Id into vendorGroup
                            select new ReturnedItemsPerVendorReportLine
                            {
                                VendorId = vendorGroup.Key,
                                VendorName = vendorGroup.FirstOrDefault().v.Name,
                                TotalReturnedItems = vendorGroup.Sum(x => x.rr.ReturnedQuantity),
                                TotalReturnedAmount = vendorGroup.Sum(x => x.rr.ReturnedQuantity * x.oi.PriceExclTax)
                            }).OrderByDescending(x=>x.TotalReturnedItems)
                            .ToListAsync();

        var rankedRankedOrderData = report.Select((x, index) => new ReturnedItemsPerVendorReportLine
        {
            VendorId = x.VendorId,
            VendorName = x.VendorName,
            TotalReturnedAmount = x.TotalReturnedAmount,
            TotalReturnedItems = x.TotalReturnedItems,
            Rank = index + 1
        });
        var result = await rankedRankedOrderData.AsQueryable().ToPagedListAsync(pageIndex, pageSize);

        return result;
    }


    public async Task<IPagedList<ReturnTableCustomerModel>> GetRefundedReturnsReportAsync(DateTime? startDate, DateTime? endDate, int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var returnRequests = _returnRequestRepository.Table;

        if (startDate.HasValue)
            returnRequests = returnRequests.Where(r => r.CreatedOnUtc >= startDate.Value);

        if (endDate.HasValue)
            returnRequests = returnRequests.Where(r => r.CreatedOnUtc <= endDate.Value);

        var report =  (from rr in returnRequests
                            join oi in _orderItemRepository.Table on rr.OrderItemId equals oi.Id
                            join p in _productRepository.Table on oi.ProductId equals p.Id
                            join o in _orderRepository.Table on oi.OrderId equals o.Id
                            join sh in _shipmentRepository.Table on o.Id equals sh.OrderId
                            join c in _customerRepository.Table on o.CustomerId equals c.Id
                            where o.PaymentStatusId == (int)PaymentStatus.Refunded || o.PaymentStatusId == (int)PaymentStatus.PartiallyRefunded
                            select new ReturnTableCustomerModel
                            {
                                OrderID = o.Id,
                                OrderDate = o.CreatedOnUtc,
                                DeliveryDate = sh.DeliveryDateUtc,
                                CustomerName = c.FirstName+ " " + c.LastName,
                                RequestId = rr.Id,
                                RefundedStatus = ((ReturnRequestStatus) rr.ReturnRequestStatusId).ToString(),
                                ReturnedQuantity = rr.ReturnedQuantity,
                                Product = p.Name
                                
                            }).OrderByDescending(x => x);
                            

        var result = await report.ToPagedListAsync(pageIndex, pageSize);

        return result;
    }


    public async Task<IPagedList<ReturnTableVendorModel>> GetRefundedReturnsReportforVendorsAsync(DateTime? startDate, DateTime? endDate, bool isClaimed, int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var returnRequests = _returnRequestRepository.Table;

        if (startDate.HasValue)
            returnRequests = returnRequests.Where(r => r.CreatedOnUtc >= startDate.Value);

        if (endDate.HasValue)
            returnRequests = returnRequests.Where(r => r.CreatedOnUtc <= endDate.Value);

        if (isClaimed)
            returnRequests = returnRequests.Where(x => x.ReturnedToVendorStatusId == (int)ReturnToVendorStatus.Claimed);
        else
            returnRequests = returnRequests.Where(x => x.ReturnedToVendorStatusId != (int)ReturnToVendorStatus.Claimed);



        var report = (from rr in returnRequests
                      join oi in _orderItemRepository.Table on rr.OrderItemId equals oi.Id
                      join p in _productRepository.Table on oi.ProductId equals p.Id
                      join o in _orderRepository.Table on oi.OrderId equals o.Id
                      join sh in _shipmentRepository.Table on o.Id equals sh.OrderId
                      join v in _vendorRepository.Table on p.VendorId equals v.Id
                      where o.PaymentStatusId == (int)PaymentStatus.Refunded || o.PaymentStatusId == (int)PaymentStatus.PartiallyRefunded
                      select new ReturnTableVendorModel
                      {
                          OrderID = o.Id,
                          OrderDate = o.CreatedOnUtc,
                          RequestId = rr.Id,
                          RefundedStatus = ((ReturnToVendorStatus)rr.ReturnedToVendorStatusId).ToString(),
                          ReturnedQuantity = rr.ReturnedQuantity,
                          Product = p.Name,
                          VendorID = v.Id,
                          VendorName =v.Name,
                          ReturnDate = sh.DeliveryDateUtc,
                          Amount = rr.ReturnedQuantity * p.ProductCost


                      }).OrderByDescending(x => x);


        var result = await report.ToPagedListAsync(pageIndex, pageSize);

        return result;
    }



    public async Task<ReturnKpiModel> GetRefundedReturnsReportKPIsAsync(DateTime? startDate, DateTime? endDate, int pageIndex = 0, int pageSize = int.MaxValue)
    {
        // Define date range
        var returnRequests = _returnRequestRepository.Table;

        if (startDate.HasValue)
            returnRequests = returnRequests.Where(r => r.CreatedOnUtc >= startDate.Value);

        if (endDate.HasValue)
            returnRequests = returnRequests.Where(r => r.CreatedOnUtc <= endDate.Value);
        // KPIs
        var kpiQuery = from rr in returnRequests
                       join oi in _orderItemRepository.Table on rr.OrderItemId equals oi.Id
                       join o in _orderRepository.Table on oi.OrderId equals o.Id
                       group new { rr, o} by 1 into g // Dummy grouping to aggregate over all rows
                       select new ReturnKpiModel
                       {
                           TotalRefundedOnWallet = g.Sum(r => r.rr.ReturnRequestStatusId == (int)ReturnRequestStatus.RefundedToWallet ? r.o.RefundedAmount : 0),
                           TotalRefundedOnCard = g.Sum(r => r.rr.ReturnRequestStatusId == (int)ReturnRequestStatus.RefundedToCard ? r.o.RefundedAmount : 0),
                           TotalRefundedAmount = g.Sum(r => r.o.RefundedAmount)
                       };

        var kpis = await kpiQuery.FirstOrDefaultAsync();
        return kpis;
    }

    #endregion
}