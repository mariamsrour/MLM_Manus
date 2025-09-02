using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Forums;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Stores;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Forums;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Web.Factories;
using Nop.Web.Framework.Controllers;
using Nop.Web.Models.Catalog;
using Nop.Web.Models.PrivateMessages;
using Nop.Web.Models.Vendors;
using NUglify.Helpers;

namespace Nop.Web.Controllers;

[AutoValidateAntiforgeryToken]
public partial class PrivateMessagesController : BasePublicController
{
    #region Fields

    protected readonly ForumSettings _forumSettings;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ICustomerService _customerService;
    protected readonly IForumService _forumService;
    protected readonly ILocalizationService _localizationService;
    protected readonly IPrivateMessagesModelFactory _privateMessagesModelFactory;
    protected readonly IStoreContext _storeContext;
    protected readonly IWorkContext _workContext;
    protected readonly IProductService _productService;
    protected readonly IProductModelFactory _productModelFactory;
    protected readonly IVendorModelFactory _vendorModelFactory;
    protected readonly IPictureService _pictureService;
    protected readonly MediaSettings _mediaSettings;
    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly CustomerSettings _customerSettings;


    #endregion

    #region Ctor

    public PrivateMessagesController(ForumSettings forumSettings,
        ICustomerActivityService customerActivityService,
        ICustomerService customerService,
        IForumService forumService,
        ILocalizationService localizationService,
        IPrivateMessagesModelFactory privateMessagesModelFactory,
        IStoreContext storeContext,
        IWorkContext workContext,
        IProductService productService,
        IProductModelFactory productModelFactory,
        IVendorModelFactory vendorModelFactory,
        IPictureService pictureService,
        MediaSettings mediaSettings,
        IGenericAttributeService genericAttributeService,
        CustomerSettings customerSettings
)
    {
        _forumSettings = forumSettings;
        _customerActivityService = customerActivityService;
        _customerService = customerService;
        _forumService = forumService;
        _localizationService = localizationService;
        _privateMessagesModelFactory = privateMessagesModelFactory;
        _storeContext = storeContext;
        _workContext = workContext;
        _productService = productService;
        _productModelFactory = productModelFactory;
        _vendorModelFactory = vendorModelFactory;
        _pictureService = pictureService;
        _mediaSettings = mediaSettings;
        _genericAttributeService = genericAttributeService;
        _customerSettings = customerSettings;
    }

    #endregion

    #region Methods

    public virtual async Task<IActionResult> Index(int? pageNumber, string tab)
    {
        if (!_forumSettings.AllowPrivateMessages)
        {
            return RedirectToRoute("Homepage");
        }

        if (await _customerService.IsGuestAsync(await _workContext.GetCurrentCustomerAsync()))
        {
            return Challenge();
        }

        var model = await _privateMessagesModelFactory.PreparePrivateMessageIndexModelAsync(pageNumber, tab);
        return View(model);
    }

    public virtual async Task<IActionResult> MyMessages()
    {
        if (await _customerService.IsGuestAsync(await _workContext.GetCurrentCustomerAsync()))
        {
            return Challenge();
        }

        return View();
    }
    

    [HttpPost, FormValueRequired("delete-inbox"), ActionName("InboxUpdate")]
    public virtual async Task<IActionResult> DeleteInboxPM(IFormCollection formCollection)
    {
        foreach (var key in formCollection.Keys)
        {
            var value = formCollection[key];

            if (value.Equals("on") && key.StartsWith("pm", StringComparison.InvariantCultureIgnoreCase))
            {
                var id = key.Replace("pm", "").Trim();
                if (int.TryParse(id, out var privateMessageId))
                {
                    var pm = await _forumService.GetPrivateMessageByIdAsync(privateMessageId);
                    if (pm != null)
                    {
                        var customer = await _workContext.GetCurrentCustomerAsync();

                        if (pm.ToCustomerId == customer.Id)
                        {
                            pm.IsDeletedByRecipient = true;
                            await _forumService.UpdatePrivateMessageAsync(pm);
                        }
                    }
                }
            }
        }
        return RedirectToRoute("PrivateMessages");
    }

    [HttpPost, FormValueRequired("mark-unread"), ActionName("InboxUpdate")]
    public virtual async Task<IActionResult> MarkUnread(IFormCollection formCollection)
    {
        foreach (var key in formCollection.Keys)
        {
            var value = formCollection[key];

            if (value.Equals("on") && key.StartsWith("pm", StringComparison.InvariantCultureIgnoreCase))
            {
                var id = key.Replace("pm", "").Trim();
                if (int.TryParse(id, out var privateMessageId))
                {
                    var pm = await _forumService.GetPrivateMessageByIdAsync(privateMessageId);
                    if (pm != null)
                    {
                        var customer = await _workContext.GetCurrentCustomerAsync();

                        if (pm.ToCustomerId == customer.Id)
                        {
                            pm.IsRead = false;
                            await _forumService.UpdatePrivateMessageAsync(pm);
                        }
                    }
                }
            }
        }
        return RedirectToRoute("PrivateMessages");
    }

    //updates sent items (deletes PrivateMessages)
    [HttpPost, FormValueRequired("delete-sent"), ActionName("SentUpdate")]
    public virtual async Task<IActionResult> DeleteSentPM(IFormCollection formCollection)
    {
        foreach (var key in formCollection.Keys)
        {
            var value = formCollection[key];

            if (value.Equals("on") && key.StartsWith("si", StringComparison.InvariantCultureIgnoreCase))
            {
                var id = key.Replace("si", "").Trim();
                if (int.TryParse(id, out var privateMessageId))
                {
                    var pm = await _forumService.GetPrivateMessageByIdAsync(privateMessageId);
                    if (pm != null)
                    {
                        var customer = await _workContext.GetCurrentCustomerAsync();

                        if (pm.FromCustomerId == customer.Id)
                        {
                            pm.IsDeletedByAuthor = true;
                            await _forumService.UpdatePrivateMessageAsync(pm);
                        }
                    }
                }
            }
        }
        return RedirectToRoute("PrivateMessages", new { tab = "sent" });
    }

    public virtual async Task<IActionResult> SendPM(int toCustomerId,int adId, int? replyToMessageId)
    {
        if (!_forumSettings.AllowPrivateMessages)
            return RedirectToRoute("Homepage");

        if (await _customerService.IsGuestAsync(await _workContext.GetCurrentCustomerAsync()))
            return Challenge();

        var customerTo = await _customerService.GetCustomerByIdAsync(toCustomerId);
        if (customerTo == null || await _customerService.IsGuestAsync(customerTo))
            return RedirectToRoute("PrivateMessages");

        PrivateMessage replyToPM = null;
        if (replyToMessageId.HasValue)
        {
            //reply to a previous PM
            replyToPM = await _forumService.GetPrivateMessageByIdAsync(replyToMessageId.Value);
        }

        var model = await _privateMessagesModelFactory.PrepareSendPrivateMessageModelAsync(customerTo, replyToPM);
        var adPost = await _productService.GetProductByIdAsync(adId);
        model.Subject = adPost.Name;
        var pictureModels = await _productModelFactory.PrepareProductOverviewPicturesModelAsync(adPost, null);

        model.PictureId = pictureModels.FirstOrDefault()?.Id ?? 0;
        model.PictureUrl = pictureModels.FirstOrDefault()?.ImageUrl ?? string.Empty;
        model.VendorId = adPost.VendorId;
        model.AdId = adId;  
        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> SendPM(SendPrivateMessageModel model)
    {
        if (!_forumSettings.AllowPrivateMessages)
        {
            return RedirectToRoute("Homepage");
        }

        var customer = await _workContext.GetCurrentCustomerAsync();
        if (await _customerService.IsGuestAsync(customer))
        {
            return Challenge();
        }

        Customer toCustomer;
      
            //first PM
            toCustomer = await _customerService.GetCustomerByIdAsync(model.ToCustomerId);
        

        if (toCustomer == null || await _customerService.IsGuestAsync(toCustomer))
        {
            return RedirectToRoute("CustomerMessages");
        }

        if (ModelState.IsValid)
        {
            try
            {
                var subject = model.Subject;
             

                var text = model.Message;
               

                var nowUtc = DateTime.UtcNow;
                var store = await _storeContext.GetCurrentStoreAsync();


                var privateMessage = new PrivateMessage
                {
                    StoreId = store.Id,
                    ToCustomerId = toCustomer.Id,
                    FromCustomerId = customer.Id,
                    Subject = subject,
                    Text = text,
                    IsDeletedByAuthor = false,
                    IsDeletedByRecipient = false,
                    IsRead = false,
                    CreatedOnUtc = nowUtc,
                    AdId = model.AdId,
                    PictureId = model.PictureId 

                };

                await _forumService.InsertPrivateMessageAsync(privateMessage);

                //activity log
                await _customerActivityService.InsertActivityAsync("PublicStore.SendPM",
                    string.Format(await _localizationService.GetResourceAsync("ActivityLog.PublicStore.SendPM"), toCustomer.Email), toCustomer);

                return RedirectToRoute("ViewPm", new { adId = model.AdId, from = customer.Id , to= model.ToCustomerId});
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
        }

        model = await _privateMessagesModelFactory.PrepareSendPrivateMessageModelAsync(toCustomer, null);
        return View(model);
    }

    public virtual async Task<IActionResult> ViewPM(int adId, int from, int to )
    {
        if (!_forumSettings.AllowPrivateMessages)
        {
            return RedirectToRoute("Homepage");
        }

        var customer = await _workContext.GetCurrentCustomerAsync();
        var store = customer.RegisteredInStoreId;
        if (await _customerService.IsGuestAsync(customer))
        {
            return Challenge();
        }


        var list = await _forumService.GetConversationForAdAsync(from,to,adId,store);
        var messages = new List<PrivateMessageModel>();
        
        foreach (var m in list)
            messages.Add(await _privateMessagesModelFactory.PreparePrivateMessageModelAsync(m));
        var fromCustomer = await _customerService.GetCustomerByIdAsync(from);
        var toCustomer = await _customerService.GetCustomerByIdAsync(to);
        if(fromCustomer.Id == customer.Id)
        {
            var vendorModel = await _vendorModelFactory.PrepareVendorInfoModelAsync(new VendorInfoModel(), false, vendorId: toCustomer.VendorId);
            ViewBag.vendor = vendorModel;
        } else
        {
            var vendorModel = await _vendorModelFactory.PrepareVendorInfoModelAsync(new VendorInfoModel(), false, vendorId: customer.VendorId);
            ViewBag.vendor = vendorModel;
        }
        

        var toCustomerPictureUrl = await _pictureService.GetPictureUrlAsync(
                await _genericAttributeService.GetAttributeAsync<int>(toCustomer, NopCustomerDefaults.AvatarPictureIdAttribute),
                _mediaSettings.AvatarPictureSize,
                _customerSettings.DefaultAvatarEnabled,
                defaultPictureType: PictureType.Avatar);
        ;
        var fromCustomerPictureUrl = await _pictureService.GetPictureUrlAsync(
                await _genericAttributeService.GetAttributeAsync<int>(fromCustomer, NopCustomerDefaults.AvatarPictureIdAttribute),
                _mediaSettings.AvatarPictureSize,
                _customerSettings.DefaultAvatarEnabled,
                defaultPictureType: PictureType.Avatar);
        var ad = await _productService.GetProductByIdAsync(adId);
        var adPictureId =  (await _pictureService.GetPicturesByProductIdAsync(adId)).FirstOrDefault().Id;
        var adPictureUrl = await _pictureService.GetPictureUrlAsync(adPictureId);
        var model = new PrivateMessageThreadModel()
        {
            FromCustomerId = fromCustomer.Id,
            CustomerFromName = await _customerService.FormatUsernameAsync(fromCustomer),
            ToCustomerId = toCustomer.Id,
            CustomerToName = await _customerService.FormatUsernameAsync(toCustomer),
            Subject = ad.Name,
            AdId = ad.Id,
            AdPictureId = adPictureId,
            AdPictureUrl = adPictureUrl,
            ToCustomerPictureUrl = toCustomerPictureUrl,
            FromCustomerPictureUrl = fromCustomerPictureUrl,
            Messages = messages

        };
        // update is read status

        foreach (var mm in model.Messages.Where(x=>!x.IsRead && x.ToCustomerId == customer.Id))
        {
            if (mm.ToCustomerId == customer.Id )
            {
                var pm = await _forumService.GetPrivateMessageByIdAsync(mm.Id);
                pm.IsRead = true;
                await _forumService.UpdatePrivateMessageAsync(pm);
                mm.IsRead = true;
            }
        }

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> DeletePM(int adId, int from, int to)
    {
        //if (!_forumSettings.AllowPrivateMessages)
        //{
        //    return RedirectToRoute("Homepage");
        //}

        var customer = await _workContext.GetCurrentCustomerAsync();
        var store = customer.RegisteredInStoreId;

        if (await _customerService.IsGuestAsync(customer))
        {
            return Challenge();
        }
        var list = await _forumService.GetConversationForAdAsync(from, to, adId, store);

        foreach(var pm in list) {

            if (pm != null)
            {
                if (pm.FromCustomerId == customer.Id)
                {
                    pm.IsDeletedByAuthor = true;
                    await _forumService.UpdatePrivateMessageAsync(pm);
                }

                if (pm.ToCustomerId == customer.Id)
                {
                    pm.IsDeletedByRecipient = true;
                    await _forumService.UpdatePrivateMessageAsync(pm);
                }
            }
        }
        return RedirectToRoute("PrivateMessages");
    }


    public virtual async Task<IActionResult> BlockPM(int adId, int from, int to)
    {

        var customer = await _workContext.GetCurrentCustomerAsync();
        var store = customer.RegisteredInStoreId;

        if (await _customerService.IsGuestAsync(customer))
        {
            return Challenge();
        }
        var list = await _forumService.GetConversationForAdAsync(from, to, adId, store);

        foreach (var pm in list)
        {

            await _forumService.DeletePrivateMessageAsync(pm);
        }
        return Ok();
    }


    #endregion
}