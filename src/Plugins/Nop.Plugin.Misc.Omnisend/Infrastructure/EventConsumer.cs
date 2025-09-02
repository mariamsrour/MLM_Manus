using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Vendors;
using Nop.Core.Events;
using Nop.Plugin.Misc.Omnisend.Services;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Events;
using Nop.Services.Vendors;
using Nop.Web.Framework.Events;

namespace Nop.Plugin.Misc.Omnisend.Infrastructure;

/// <summary>
/// Represents plugin event consumer
/// </summary>
internal class EventConsumer : IConsumer<CustomerLoggedinEvent>,
    IConsumer<CustomerRegisteredEvent>,
    IConsumer<EmailSubscribedEvent>,
    IConsumer<EmailUnsubscribedEvent>,
    IConsumer<EntityDeletedEvent<ProductAttributeCombination>>,
    IConsumer<EntityDeletedEvent<ShoppingCartItem>>,
    IConsumer<EntityUpdatedEvent<Product>>,
    IConsumer<EntityInsertedEvent<ProductAttributeCombination>>,
    IConsumer<EntityInsertedEvent<ShoppingCartItem>>,
    IConsumer<EntityInsertedEvent<StockQuantityHistory>>,
    IConsumer<EntityUpdatedEvent<ShoppingCartItem>>,
    IConsumer<EntityInsertedEvent<OrderItem>>,
    IConsumer<OrderAuthorizedEvent>,
    IConsumer<OrderPaidEvent>,
    IConsumer<OrderPlacedEvent>,
    IConsumer<OrderRefundedEvent>,
    IConsumer<OrderStatusChangedEvent>,
    IConsumer<OrderVoidedEvent>,
    IConsumer<PageRenderingEvent>
{
    #region Fields

    private readonly IGenericAttributeService _genericAttributeService;
    private readonly OmnisendEventsService _omnisendEventsService;
    private readonly OmnisendService _omnisendService;
    private readonly OmnisendSettings _settings;
    private readonly IVendorService _vendorService;
    private readonly ICustomerService _customerService;
    private readonly VendorSettings _vendorSettings;

    #endregion

    #region Ctor

    public EventConsumer(IGenericAttributeService genericAttributeService,
        OmnisendEventsService omnisendEventsService,
        OmnisendService omnisendService,
        OmnisendSettings settings,
         IVendorService vendorService,          // New
    ICustomerService customerService,      // New
    VendorSettings vendorSettings)         // New
    {
        _genericAttributeService = genericAttributeService;
        _omnisendEventsService = omnisendEventsService;
        _omnisendService = omnisendService;
        _settings = settings;
        // New vendor-related dependencies
        _vendorService = vendorService;
        _customerService = customerService;
        _vendorSettings = vendorSettings;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Handle event
    /// </summary>
    /// <param name="eventMessage">Event</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(CustomerLoggedinEvent eventMessage)
    {
        if (string.IsNullOrEmpty(_settings.IdentifyContactScript))
            return;

        var script = _settings.IdentifyContactScript.Replace(OmnisendDefaults.Email, eventMessage.Customer.Email);

        await _genericAttributeService.SaveAttributeAsync(eventMessage.Customer,
            OmnisendDefaults.IdentifyContactAttribute, script);
    }

    /// <summary>
    /// Handle event
    /// </summary>
    /// <param name="eventMessage">Event</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(CustomerRegisteredEvent eventMessage)
    {
        await _omnisendService.UpdateContactAsync(eventMessage.Customer);
        // New vendor registration logic
        var customer = eventMessage.Customer;

        // Check if customer is already a vendor
        if (customer.VendorId > 0)
            return;

        // Create new vendor
        var vendor = new Vendor
        {
            Name = $"{customer.FirstName} {customer.LastName}",
            Email = customer.Email,
            Active =true,
            AllowCustomersToSelectPageSize = true,
            PageSize = 15,
            CreatedOnUtc = customer.CreatedOnUtc,
            AddressId = customer.BillingAddressId ?? customer.ShippingAddressId ?? 0,
            Phone = customer.Phone,
            
        };

        // Save vendor
        await _vendorService.InsertVendorAsync(vendor);

        // Link customer to vendor
        customer.VendorId = vendor.Id;
        await _customerService.UpdateCustomerAsync(customer);
    }

    /// <summary>
    /// Handle event
    /// </summary>
    /// <param name="eventMessage">Event</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EmailSubscribedEvent eventMessage)
    {
        await _omnisendService.UpdateOrCreateContactAsync(eventMessage.Subscription, true);
    }

    /// <summary>
    /// Handle event
    /// </summary>
    /// <param name="eventMessage">Event</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EmailUnsubscribedEvent eventMessage)
    {
        await _omnisendService.UpdateOrCreateContactAsync(eventMessage.Subscription);
    }

    /// <summary>
    /// Handle event
    /// </summary>
    /// <param name="eventMessage">Event</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityInsertedEvent<StockQuantityHistory> eventMessage)
    {
        var history = eventMessage.Entity;

        await _omnisendService.UpdateProductAsync(history.ProductId);
    }

    /// <summary>
    /// Handle event
    /// </summary>
    /// <param name="eventMessage">Event</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityInsertedEvent<ProductAttributeCombination> eventMessage)
    {
        await _omnisendService.UpdateProductAsync(eventMessage.Entity.ProductId);
    }

    /// <summary>
    /// Handle event
    /// </summary>
    /// <param name="eventMessage">Event</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityDeletedEvent<ProductAttributeCombination> eventMessage)
    {
        await _omnisendService.UpdateProductAsync(eventMessage.Entity.ProductId);
    }

    /// <summary>
    /// Handle event
    /// </summary>
    /// <param name="eventMessage">Event</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityInsertedEvent<ShoppingCartItem> eventMessage)
    {
        var entity = eventMessage.Entity;

        if (entity.ShoppingCartType != ShoppingCartType.ShoppingCart)
            return;

        await _omnisendEventsService.SendAddedProductToCartEventAsync(entity);
        //await _omnisendService.AddShoppingCartItemAsync(eventMessage.Entity);
    }

    /// <summary>
    /// Handle event
    /// </summary>
    /// <param name="eventMessage">Event</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(OrderPlacedEvent eventMessage)
    {
        await _omnisendEventsService.SendOrderPlacedEventAsync(eventMessage.Order);
        await _omnisendService.PlaceOrderAsync(eventMessage.Order);
    }

    /// <summary>
    /// Handle event
    /// </summary>
    /// <param name="eventMessage">Event</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(OrderPaidEvent eventMessage)
    {
        await _omnisendEventsService.SendOrderPaidEventAsync(eventMessage);
        //await _omnisendService.UpdateOrderAsync(eventMessage.Order);
    }

    /// <summary>
    /// Handle event
    /// </summary>
    /// <param name="eventMessage">Event</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(OrderRefundedEvent eventMessage)
    {
        await _omnisendEventsService.SendOrderRefundedEventAsync(eventMessage);
        //await _omnisendService.UpdateOrderAsync(eventMessage.Order);
    }

    /// <summary>
    /// Handle event
    /// </summary>
    /// <param name="eventMessage">Event</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(OrderStatusChangedEvent eventMessage)
    {
        await _omnisendEventsService.SendOrderStatusChangedEventAsync(eventMessage);
        //await _omnisendService.UpdateOrderAsync(eventMessage.Order);
    }

    /// <summary>
    /// Handle event
    /// </summary>
    /// <param name="eventMessage">Event</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(PageRenderingEvent eventMessage)
    {
        await _omnisendEventsService.SendStartedCheckoutEventAsync(eventMessage);
    }

    /// <summary>
    /// Handle event
    /// </summary>
    /// <param name="eventMessage">Event</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public Task HandleEventAsync(EntityUpdatedEvent<ShoppingCartItem> eventMessage)
    {
        //await _omnisendService.EditShoppingCartItemAsync(eventMessage.Entity);
            
        return Task.CompletedTask;
    }

    /// <summary>
    /// Handle event
    /// </summary>
    /// <param name="eventMessage">Event</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityDeletedEvent<ShoppingCartItem> eventMessage)
    {
        await _omnisendService.DeleteShoppingCartItemAsync(eventMessage.Entity);
    }

    /// <summary>
    /// Handle event
    /// </summary>
    /// <param name="eventMessage">Event</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public Task HandleEventAsync(OrderAuthorizedEvent eventMessage)
    {
        //await _omnisendService.UpdateOrderAsync(eventMessage.Order);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Handle event
    /// </summary>
    /// <param name="eventMessage">Event</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public Task HandleEventAsync(OrderVoidedEvent eventMessage)
    {
        //await _omnisendService.UpdateOrderAsync(eventMessage.Order);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Handle event
    /// </summary>
    /// <param name="eventMessage">Event</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityInsertedEvent<OrderItem> eventMessage)
    {
        await _omnisendService.OrderItemAddedAsync(eventMessage.Entity);
    }

    /// <summary>
    /// Handle event
    /// </summary>
    /// <param name="eventMessage">Event</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityUpdatedEvent<Product> eventMessage)
    {
        await _omnisendService.CreateOrUpdateProductAsync(eventMessage.Entity);
    }

    #endregion
}