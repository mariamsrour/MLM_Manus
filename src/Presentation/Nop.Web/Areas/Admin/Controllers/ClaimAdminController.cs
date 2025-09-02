using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Common;
using Nop.Core;
using Nop.Core.Events;
using Nop.Services.Common;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.News;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.Models.Common;
using Nop.Web.Areas.Admin.Models.Catalog;
using Microsoft.AspNetCore.Mvc.Rendering;
using DocumentFormat.OpenXml.Office2010.Excel;
using Nop.Web.Framework.Models.DataTables;
using Nop.Web.Framework.Models.Extensions;
using Nop.Services.Customers;
using Nop.Core.Domain.Customers;
using NUglify.Helpers;
using NPOI.OpenXmlFormats.Spreadsheet;

namespace Nop.Web.Areas.Admin.Controllers;
public class ClaimAdminController : BaseAdminController
{
    #region Fields


    private readonly IClaimService _claimService;
    private readonly IPictureService _pictureService;
    private readonly IReportService _reportService;
    private readonly ICustomerService _customerService;
    private readonly IWorkContext _workContext;
    private readonly ILanguageService _languageService;
    private readonly ILocalizationService _localizationService;
    #endregion

    #region Ctor

    public ClaimAdminController(IClaimService claimService, IPictureService pictureService, IReportService reportService, 
        ICustomerService customerService, IWorkContext workContext,
        ILanguageService languageService, ILocalizationService localizationService
        )
    {
        _claimService = claimService;
        _pictureService = pictureService;
        _reportService = reportService;
        _customerService = customerService;
        _workContext = workContext;
        _languageService = languageService;
        _localizationService = localizationService;
    }

    public virtual IActionResult Index()
    {
        return RedirectToAction("List");
    }

    public virtual IActionResult Reports()
    {
        return RedirectToAction("ListReports");
    }

    public virtual IActionResult List()
    {

        //prepare model
        var model = new ClaimSearchModel()
        {
            ClaimStatusOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Select Status", Value = "0" },
                new SelectListItem { Text = ClaimStatus.Closed.ToString(), Value = "2" },
                new SelectListItem { Text = ClaimStatus.Opened.ToString(), Value = "1" },
            },
            Search = string.Empty,
            Status = 0, // Default to Opened claims
            Startdate = null,
            End = null

        };
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> List(ClaimSearchModel searchModel)
    {
        var status = searchModel.Status > 0 ? (ClaimStatus)searchModel.Status : 0;

        IPagedList<CustomerClaims> claims = await _claimService.GetAllClaimsAsync(
            searchModel.Search,
            status == 0 ? null : status,
            searchModel.Startdate,
            searchModel.End,
            searchModel.Page - 1,
            searchModel.PageSize
        );

        var model =  new ClaimAdminListModel().PrepareToGrid(searchModel, claims, () =>
        {
            return  claims.Select(c => new ClaimItems
            {
                Id = c.Id,
                Title = c.Title,
                Name = c.Username,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Message = c.Message,
                CreatedOnUtc = c.CreatedOnUtc,
                Status = (ClaimStatus)c.Status,
                PictureIds = c.AttachemntIds?.Split(',').Select(int.Parse).ToList() ?? new List<int>()
            }).ToList(); // Materialize to list
        });

        return Json(model);

    }


    public async Task<IActionResult> Edit(int id)
    {
        var c = await _claimService.GetClaimByIdAsync(id);
        if (c == null)
            return RedirectToAction("List");
        ViewBag.ClaimStatusOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = ClaimStatus.Closed.ToString(), Value = "2" },
                new SelectListItem { Text = ClaimStatus.Opened.ToString(), Value = "1" },
            };
        var picIds = c.AttachemntIds?.Split(',').Select(int.Parse).ToList() ?? new List<int>();
        List<string> picUrls = new List<string>();
        foreach ( var pic in picIds )
        {
            var url = await _pictureService.GetPictureUrlAsync(pic);
            picUrls.Add(url);
        }
        var model = new ClaimItems
        {
            Id = c.Id,
            Title = c.Title,
            Name = c.Username,
            PhoneNumber = c.PhoneNumber,
            Email = c.Email,
            Message = c.Message,
            CreatedOnUtc = c.CreatedOnUtc,
            Status = (ClaimStatus)c.Status,
            PictureIds = picIds,
            PictureUrls = picUrls,
            AdId = c.AdId,
            
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ClaimItems model)
    {
        var claim = await _claimService.GetClaimByIdAsync(model.Id);
        if (claim == null)
            return NotFound();

        claim.Status = (int)model.Status;
        claim.AdminNote = model.AdminNote;
        claim.ModifiedBy = (await _workContext.GetCurrentCustomerAsync()).Id;
        await _claimService.UpdateClaimAsync(claim);
        return RedirectToAction("Index");
    }

    public virtual async  Task<IActionResult> ListReports()
    {
        // new SelectListItem { Text = ClaimStatus.Opened.ToString(), Value = "1" },
        var lang = await _workContext.GetWorkingLanguageAsync();
        var res = (await _reportService.GetAllReportReasonsAsync()).Where(x=>x.LanguageId == lang.Id);
        //prepare model
        var model = new ReportSearchModel()
        {
            ReportReasons = res.Select(x => new SelectListItem()
            {
                Text = x.Title,
                Value = x.Id.ToString()
            }).ToList(),
            Search = string.Empty,
            Startdate = null,
            End = null

        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ListReports(ReportSearchModel searchModel)
    {
        var claims = await _reportService.GetAllReportsAsync(
            searchModel.ReportReason,
            searchModel.Startdate,
            searchModel.End,
            searchModel.Page - 1,
            searchModel.PageSize
        );

        // Materialize your mapped items
        var repList = new List<ReportItems>();

        foreach (var c in claims)
        {
            var customer = await _customerService.GetCustomerByIdAsync(c.CustomerId);
            var reason = await _reportService.GetResonById(c.ReportReason);

            repList.Add(new ReportItems
            {
                Id = c.Id,
                AdId = c.AdId,
                Description = c.Description,
                ReportReason = c.ReportReason,
                CreatedOnUtc = c.CreatedOnUtc,
                CustomerId = c.CustomerId,
                CustomerName = string.IsNullOrWhiteSpace(customer?.FirstName) ? "Anonymous" : customer.FirstName + " " + customer.LastName,
                ReportReasonName = reason.Title
            });
        }

        // Wrap your transformed list in a paged list
        var pagedResult = new PagedList<ReportItems>(
            repList,
            claims.PageIndex,
            claims.PageSize,
            claims.TotalCount
        );

        // Prepare grid model
        var model = new ReportAdminListModel().PrepareToGrid(searchModel, claims, () => pagedResult);
        return Json(model);
    }

    public async Task<IActionResult> EditReports(int id)
    {
        var c = await _reportService.GetReportByIdAsync(id);
        if (c == null)
            return RedirectToAction("ListReports");
        
        ViewBag.ClaimStatusOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = ClaimStatus.Closed.ToString(), Value = "2" },
                new SelectListItem { Text = ClaimStatus.Opened.ToString(), Value = "1" },
            };
     var cus = await _customerService.GetCustomerByIdAsync(c.CustomerId);
        var model = new ReportItems
        {
            AdId = c.AdId,
            Description = c.Description,
            CreatedOnUtc = c.CreatedOnUtc,
            CustomerId = c.CustomerId,
            CustomerName = string.IsNullOrWhiteSpace(cus.FirstName) ? "Anonymous" :cus.FirstName + " " + cus.LastName,
            ReportReason = c.ReportReason,
            ReportReasonName = (await _reportService.GetResonById(c.ReportReason)).Title
        };
        return View(model);
    }

    public IActionResult ListReasons()
    {
        var model = new ReportReasonSearchModel();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ListReasons(ReportReasonSearchModel searchModel)
    {
        var reasons = await _reportService.GetAllReportReasonsAsync();
        var ll = (await _languageService.GetAllLanguagesAsync());
        var model = new ReportReasonListModel
        {
            Data = reasons.Select(x => new ReportReasons
            {
                Id = x.Id,
                Reason = x.Title,
                LanguageId = x.LanguageId,
                Language = ll.FirstOrDefault(l => l.Id == x.LanguageId)?.Name ?? "Unknown", 
            }).ToList(),
            RecordsTotal = reasons.Count(),
            RecordsFiltered = reasons.Count(),
            Draw = "1"
        };

        return Json(model);
    }

    #region Create

    public async Task<IActionResult> CreateReason()
    {
        var model = new ReportReasons
        {
            AvailableLanguages = (await _languageService.GetAllLanguagesAsync())
                .Select(l => new SelectListItem { Text = l.Name, Value = l.Id.ToString() }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReason(ReportReasons model, bool continueEditing)
    {
        if (!ModelState.IsValid)
        {
            model.AvailableLanguages = (await _languageService.GetAllLanguagesAsync())
                .Select(l => new SelectListItem { Text = l.Name, Value = l.Id.ToString() }).ToList();

            return View(model);
        }

        await _reportService.InsertReportResonAsync(new AdReportReason
        {
            Title = model.Reason,
            LanguageId = model.LanguageId,
        });


        return   RedirectToAction("ListReasons");
    }

    #endregion

    #region Edit

    //public async Task<IActionResult> EditReason(int id)
    //{
    //    var reason = await _reportService.getrer(id);
    //    if (reason == null)
    //        return RedirectToAction("ListReasons");

    //    var model = new ReportReasons
    //    {
    //        Id = reason.Id,
    //        Reason = reason.Reason,
    //        LanguageId = reason.LanguageId,
    //        AvailableLanguages = (await _languageService.GetAllLanguagesAsync())
    //            .Select(l => new SelectListItem { Text = l.Name, Value = l.Id.ToString() }).ToList()
    //    };

    //    return View(model);
    //}

    //[HttpPost]
    //public async Task<IActionResult> EditReason(ReportReasons model, bool continueEditing)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        model.AvailableLanguages = (await _languageService.GetAllLanguagesAsync())
    //            .Select(l => new SelectListItem { Text = l.Name, Value = l.Id.ToString() }).ToList();

    //        return View(model);
    //    }

    //    var reason = await _reportReasonService.GetByIdAsync(model.Id);
    //    if (reason == null)
    //        return RedirectToAction("ListReasons");

    //    reason.Reason = model.Reason;
    //    reason.LanguageId = model.LanguageId;

    //    await _reportReasonService.UpdateAsync(reason);

    //    SuccessNotification("Reason updated successfully");

    //    return continueEditing
    //        ? RedirectToAction("EditReason", new { id = reason.Id })
    //        : RedirectToAction("ListReasons");
    //}

    #endregion

    [HttpPost]
    public async Task<IActionResult> DeleteReason([FromForm] int id)
    {
        var reason = await _reportService.GetResonById(id);
        if (reason == null)
            return ErrorJson("Reason not found");

        await _reportService.DeleteReportResonAsync(reason);

        return Json(new { success = true, message = "Deleted successfully" });
    }


}
#endregion