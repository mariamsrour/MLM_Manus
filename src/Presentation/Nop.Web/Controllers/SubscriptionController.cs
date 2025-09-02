using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Shipping;
using Nop.Core.Http.Extensions;
using Nop.Core.Infrastructure;
using Nop.Services.Attributes;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Discounts;
using Nop.Services.Html;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Shipping;
using Nop.Services.Stores;
using Nop.Services.Subscription;
using Nop.Services.Tax;
using Nop.Web.Components;
using Nop.Web.Factories;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Mvc.Routing;
using Nop.Web.Infrastructure.Cache;
using Nop.Web.Models.Media;
using Nop.Web.Models.ShoppingCart;
using Nop.Web.Models.Subscriptions;

namespace Nop.Web.Controllers;
public class SubscriptionController : BasePublicController
{
    #region Fields

    protected readonly CaptchaSettings _captchaSettings;
    protected readonly CustomerSettings _customerSettings;
    protected readonly IAttributeParser<CheckoutAttribute, CheckoutAttributeValue> _checkoutAttributeParser;
    protected readonly IAttributeService<CheckoutAttribute, CheckoutAttributeValue> _checkoutAttributeService;
    protected readonly ICurrencyService _currencyService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ICustomerService _customerService;
    protected readonly IDiscountService _discountService;
    protected readonly IDownloadService _downloadService;
    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly IGiftCardService _giftCardService;
    protected readonly IHtmlFormatter _htmlFormatter;
    protected readonly ILocalizationService _localizationService;
    protected readonly INopFileProvider _fileProvider;
    protected readonly INopUrlHelper _nopUrlHelper;
    protected readonly INotificationService _notificationService;
    protected readonly IPermissionService _permissionService;
    protected readonly IPictureService _pictureService;
    protected readonly IPriceFormatter _priceFormatter;
    protected readonly IProductAttributeParser _productAttributeParser;
    protected readonly IProductAttributeService _productAttributeService;
    protected readonly IProductService _productService;
    protected readonly IShippingService _shippingService;
    protected readonly IShoppingCartModelFactory _shoppingCartModelFactory;
    protected readonly IShoppingCartService _shoppingCartService;
    protected readonly IStaticCacheManager _staticCacheManager;
    protected readonly IStoreContext _storeContext;
    protected readonly IStoreMappingService _storeMappingService;
    protected readonly ITaxService _taxService;
    protected readonly IUrlRecordService _urlRecordService;
    protected readonly IWebHelper _webHelper;
    protected readonly IWorkContext _workContext;
    protected readonly IWorkflowMessageService _workflowMessageService;
    protected readonly MediaSettings _mediaSettings;
    protected readonly OrderSettings _orderSettings;
    protected readonly ShoppingCartSettings _shoppingCartSettings;
    protected readonly ShippingSettings _shippingSettings;
    protected readonly ISubscriptionService _subscriptionService;
    protected readonly ICategoryService _categoryService;
    protected readonly IPdfService _pdfService;
    private static readonly char[] _separator = [','];

    #endregion

    #region Ctor

    public SubscriptionController(CaptchaSettings captchaSettings,
        CustomerSettings customerSettings,
        IAttributeParser<CheckoutAttribute, CheckoutAttributeValue> checkoutAttributeParser,
        IAttributeService<CheckoutAttribute, CheckoutAttributeValue> checkoutAttributeService,
        ICurrencyService currencyService,
        ICustomerActivityService customerActivityService,
        ICustomerService customerService,
        IDiscountService discountService,
        IDownloadService downloadService,
        IGenericAttributeService genericAttributeService,
        IGiftCardService giftCardService,
        IHtmlFormatter htmlFormatter,
        ILocalizationService localizationService,
        INopFileProvider fileProvider,
        INopUrlHelper nopUrlHelper,
        INotificationService notificationService,
        IPermissionService permissionService,
        IPictureService pictureService,
        IPriceFormatter priceFormatter,
        IProductAttributeParser productAttributeParser,
        IProductAttributeService productAttributeService,
        IProductService productService,
        IShippingService shippingService,
        IShoppingCartModelFactory shoppingCartModelFactory,
        IShoppingCartService shoppingCartService,
        IStaticCacheManager staticCacheManager,
        IStoreContext storeContext,
        IStoreMappingService storeMappingService,
        ITaxService taxService,
        IUrlRecordService urlRecordService,
        IWebHelper webHelper,
        IWorkContext workContext,
        IWorkflowMessageService workflowMessageService,
        MediaSettings mediaSettings,
        OrderSettings orderSettings,
        ShoppingCartSettings shoppingCartSettings,
        ShippingSettings shippingSettings,
        ISubscriptionService subscriptionService,
        ICategoryService categoryService,
        IPdfService pdfService)
    {
        _captchaSettings = captchaSettings;
        _customerSettings = customerSettings;
        _checkoutAttributeParser = checkoutAttributeParser;
        _checkoutAttributeService = checkoutAttributeService;
        _currencyService = currencyService;
        _customerActivityService = customerActivityService;
        _customerService = customerService;
        _discountService = discountService;
        _downloadService = downloadService;
        _genericAttributeService = genericAttributeService;
        _giftCardService = giftCardService;
        _htmlFormatter = htmlFormatter;
        _localizationService = localizationService;
        _fileProvider = fileProvider;
        _nopUrlHelper = nopUrlHelper;
        _notificationService = notificationService;
        _permissionService = permissionService;
        _pictureService = pictureService;
        _priceFormatter = priceFormatter;
        _productAttributeParser = productAttributeParser;
        _productAttributeService = productAttributeService;
        _productService = productService;
        _shippingService = shippingService;
        _shoppingCartModelFactory = shoppingCartModelFactory;
        _shoppingCartService = shoppingCartService;
        _staticCacheManager = staticCacheManager;
        _storeContext = storeContext;
        _storeMappingService = storeMappingService;
        _taxService = taxService;
        _urlRecordService = urlRecordService;
        _webHelper = webHelper;
        _workContext = workContext;
        _workflowMessageService = workflowMessageService;
        _mediaSettings = mediaSettings;
        _orderSettings = orderSettings;
        _shoppingCartSettings = shoppingCartSettings;
        _shippingSettings = shippingSettings;
        _subscriptionService = subscriptionService;
        _categoryService = categoryService;
        _pdfService = pdfService;
    }

    #endregion
    public async Task<IActionResult> Plans()
    {
        var plans = new List<Plan>();
        var res = await _subscriptionService.GetAvailablePlansWithPackagesAsync();
        foreach(var p in res)
        {
            plans.Add(new Plan()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Features = p.Features.Split(",").ToList(),
                IsPopular = p.IsPopular,
                Packages = p.Packages.Select(x => new Package()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    DurationDays = x.DurationDays,
                    HighlightedAds = x.HighlightedAds,
                    NormalAds = x.NormalAds,
                    

                }).ToList()
            });
        }
        return PartialView("_plans", plans);
    }

    public async Task<IActionResult> CustomerSubscriptions(int adId, int catId)
    {
        var id = (await _workContext.GetCurrentCustomerAsync()).Id;
        var subs = await _subscriptionService.GetActiveSubscriptionAsync(id, catId);
        var categories = await _categoryService.GetAllCategoriesAsync();

        var mode = new List<UserSubscription>();
        ViewBag.adId = adId;
        ViewBag.catId = catId;

        foreach (var x in subs)
        {
            var sub = new UserSubscription()
            {
                Id = x.Id,
                CustomerId = x.CustomerId,
                PackageId = x.PackageId,
                StartDate = x.StartDate,
                ExpirationDate = x.ExpirationDate,
                HighlightedUsed = x.HighlightedUsed,
                NormalUsed = x.NormalUsed,
                IsActive = x.IsActive,
                catId = x.CategoryId,
                catName = categories.FirstOrDefault(c => c.Id == x.CategoryId)?.Name ?? "",

            };
            var pp = (await _subscriptionService.GetAvailablePlansWithPackagesAsync()).Where(p=>p.Id == x.Package.PlanId).FirstOrDefault();
            sub.Package = new Package()
            {
                Id = x.Package.Id,
                Name = x.Package.Name,
                Price = x.Package.Price,
                DurationDays = x.Package.DurationDays,
                HighlightedAds = x.Package.HighlightedAds,
                NormalAds = x.Package.NormalAds,
                PlanId = x.Package.PlanId,
                Plan = new Plan()
                {
                    Name = pp.Name,
                    Description = pp.Description,
                    Features = pp.Features.Split(",").ToList(),
                    IsPopular = pp.IsPopular,
                }
            };
            mode.Add(sub);

        }
        var model = new List<UserSubscription>();
        return View(mode);
    }

    public async Task<IActionResult> AllSubscriptions()
    {
        var id = (await _workContext.GetCurrentCustomerAsync()).Id;
        var subs = await _subscriptionService.GetAllSubscriptionAsync(id);
        var categories = await _categoryService.GetAllCategoriesAsync();

        var mode = new List<UserSubscription>();
        foreach (var x in subs)
        {
            var sub = new UserSubscription()
            {
                Id = x.Id,
                CustomerId = x.CustomerId,
                PackageId = x.PackageId,
                StartDate = x.StartDate,
                ExpirationDate = x.ExpirationDate,
                HighlightedUsed = x.HighlightedUsed,
                NormalUsed = x.NormalUsed,
                IsActive = x.IsActive,
                catId = x.CategoryId,
                catName = categories.FirstOrDefault(c => c.Id == x.CategoryId)?.Name ?? "",


            };
            var pp = (await _subscriptionService.GetAvailablePlansWithPackagesAsync()).Where(p => p.Id == x.Package.PlanId).FirstOrDefault();
            sub.Package = new Package()
            {
                Id = x.Package.Id,
                Name = x.Package.Name,
                Price = x.Package.Price,
                DurationDays = x.Package.DurationDays,
                HighlightedAds = x.Package.HighlightedAds,
                NormalAds = x.Package.NormalAds,
                PlanId = x.Package.PlanId,
                Plan = new Plan()
                {
                    Name = pp.Name,
                    Description = pp.Description,
                    Features = pp.Features.Split(",").ToList(),
                    IsPopular = pp.IsPopular,
                }
            };
            mode.Add(sub);

        }
        var model = new List<UserSubscription>();
        return View("AllSubscriptions", mode);
    }

    public IActionResult Subscripe()
    {
        return PartialView("_subscripe");
    }

    public async Task<IActionResult> SelectPlan(int adId, int catId=0)
    {
        ViewBag.adId = adId;
        ViewBag.catId = catId;

        var plans = new List<Plan>();
        var res = await _subscriptionService.GetAvailablePlansWithPackagesAsync();
        foreach (var p in res)
        {
            var pp = new Plan()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Features = p.Features.Split(",").ToList(),
                IsPopular = p.IsPopular,
                Packages = p.Packages?.Select(x => new Package()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    DurationDays = x.DurationDays,
                    HighlightedAds = x.HighlightedAds,
                    NormalAds = x.NormalAds,
                    PlanId = x.PlanId

                }).ToList()
            };
            plans.Add(pp);
        }
        return View(plans);
    }

    public async Task<IActionResult> Checkout(int packageId, bool isRenew = false, int adId = 0, int catId=0)
    {
        ViewBag.adId = adId;
        ViewBag.catId = catId;

        ViewBag.isRenew = isRenew;  
        var plan = await _subscriptionService.GetUserSubscriptionByIdAsync(packageId);
        var model = new Plan()
        {
            Id = plan.Id,
            Name = plan.Name,
            Description = plan.Description,
            Features = plan.Features.Split(",").ToList(),
            IsPopular = plan.IsPopular,
            Packages = plan.Packages?.Select(x => new Package()
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                DurationDays = x.DurationDays,
                HighlightedAds = x.HighlightedAds,
                NormalAds = x.NormalAds,
                PlanId = x.PlanId
            }).ToList()
        };

        return View(model);
    }

    public async Task<IActionResult> CheckoutComplete(int package = 0, double price = 0, bool isNew = false, bool isRenew = false, int activeSub = 0, int adId= 0, int catId=0)
    {

        ViewBag.adId = adId;
        ViewBag.catId = catId;

        var customer = await _workContext.GetCurrentCustomerAsync();
        var lang = await _workContext.GetWorkingLanguageAsync();
        if(isNew)
        {
            var res = await _subscriptionService.CreateSubscriptionAsync(customer.Id, package, price, catId);
            if(!isRenew)
                await _subscriptionService.IncrementAdUsageAsync(res.Id);
            // send subscription invoice to customer
            //byte[] pdfBytes;
            //using (var stream = new MemoryStream())
            //{
            //    await _pdfService.PrintSubscriptionInvoiceToPdfAsync(stream, res);
            //    pdfBytes = stream.ToArray(); // get byte[] from memory
            //}
            //await _workflowMessageService.SendSubscriptionInvoiceCustomerNotificationAsync(customer, res, lang.Id, pdfBytes);
            return View(res);
        } else
        {
           // var activeSub = await _subscriptionService.GetActiveSubscriptionAsync(customer.Id);
            await _subscriptionService.IncrementAdUsageAsync(activeSub);
        }
        return View();
    }

    public async Task<IActionResult> AllBillings()
    {
        var id = (await _workContext.GetCurrentCustomerAsync()).Id;
        var subs = await _subscriptionService.GetAllSubscriptionAsync(id);
        var mode = new List<UserSubscription>();
        foreach (var x in subs)
        {
            var sub = new UserSubscription()
            {
                Id = x.Id,
                CustomerId = x.CustomerId,
                PackageId = x.PackageId,
                StartDate = x.StartDate,
                ExpirationDate = x.ExpirationDate,
                HighlightedUsed = x.HighlightedUsed,
                NormalUsed = x.NormalUsed,
                IsActive = x.IsActive,
                PaidPrice = x.PaidPrice

            };
            var pp = (await _subscriptionService.GetAvailablePlansWithPackagesAsync()).Where(p => p.Id == x.Package.PlanId).FirstOrDefault();
            sub.Package = new Package()
            {
                Id = x.Package.Id,
                Name = x.Package.Name,
                Price = x.Package.Price,
                DurationDays = x.Package.DurationDays,
                HighlightedAds = x.Package.HighlightedAds,
                NormalAds = x.Package.NormalAds,
                PlanId = x.Package.PlanId,
                Plan = new Plan()
                {
                    Name = pp.Name,
                    Description = pp.Description,
                    Features = pp.Features.Split(",").ToList(),
                    IsPopular = pp.IsPopular,
                }
            };
            mode.Add(sub);

        }
        var model = new List<UserSubscription>();
        return View(mode);
    }


}
