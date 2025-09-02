using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Media;
using Nop.Web.Factories;
using Nop.Web.Models.Common;

namespace Nop.Web.Controllers;
public class ClaimsController : BasePublicController
{
    private readonly IClaimService _claimService;
    private readonly IPictureService _pictureService;
    private readonly IDownloadService _downloadService;
    private readonly IWorkContext _workContext;
    private readonly IProductService _productService;
    private readonly IProductModelFactory _productModelFactory;
    private readonly IReportService _reportService;

    public ClaimsController(IClaimService claimService, IPictureService pictureService, 
        IDownloadService downloadService, IWorkContext workContext,
       IProductService productService, IProductModelFactory productModelFactory,
       IReportService reportService)
    {
        _claimService = claimService;
        _pictureService = pictureService;
        _downloadService = downloadService;
        _workContext = workContext;
        _productService = productService;
        _productModelFactory = productModelFactory;
        _reportService = reportService; 
    }

    [HttpGet]
    public IActionResult Add() => View(new ClaimModel());

    [HttpPost]
    public async Task<IActionResult> Add(ClaimModel model)
    {
        var currentCustomer = await _workContext.GetCurrentCustomerAsync();
        if (!ModelState.IsValid)
            return View(model);

        var pictureIds = new List<int>();
        foreach (var uploadedFile in model.Attachments)
        {
            var contentType = uploadedFile.ContentType.ToLowerInvariant();
            var vendorPictureBinary = await _downloadService.GetDownloadBitsAsync(uploadedFile);
            var picture = await _pictureService.InsertPictureAsync(vendorPictureBinary, contentType, null);
            pictureIds.Add(picture.Id);
        }

        var entity = model.ToEntity();
        entity.AttachemntIds = string.Join(",", pictureIds);
        entity.CustomerId = currentCustomer.Id;

        await _claimService.InsertClaimAsync(entity);
        return RedirectToRoute("AddedClaim"); // or thank you page
    }

    [HttpGet]
    public IActionResult ClaimAdded()
    {
        return View();
    }


    [HttpGet]
    public async Task<IActionResult> AddReport(int adId)
    {
        var model = new ReportModel();
        var ad =await  _productService.GetProductByIdAsync(adId);
        var productModel = await _productModelFactory.PrepareProductDetailsModelAsync(ad);
        var language = await _workContext.GetWorkingLanguageAsync();
        var reasons  = (await _reportService.GetAllReportReasonsAsync()).Where(x=>x.LanguageId == language.Id).ToList();

        model.Ad = productModel;
        model.AdReportReasons = reasons;
        model.AdId = adId;
        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> AddReport(ReportModel model)
    {
        if (ModelState.IsValid)
        {
            var customerId = (await _workContext.GetCurrentCustomerAsync())?.Id;
            var report = new AdReports
           {
               AdId = model.AdId,
               ReportReason = model.SelectedReasonId,
               Description = model.CustomReason,
               CreatedOnUtc = DateTime.UtcNow,
               CustomerId = customerId??0
            };
            await _reportService.InsertReportAsync(report);
            return RedirectToRoute("ReportAdded");

        }
        return View(model);

    }


    [HttpGet]
    public IActionResult ReportAdded()
    {
        return View();
    }

}
