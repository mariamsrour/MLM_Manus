using System.Text;
using DocumentFormat.OpenXml.Office2016.Drawing.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Primitives;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Vendors;
using Nop.Core.Http;
using Nop.Core.Infrastructure;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Discounts;
using Nop.Services.ExportImport;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Shipping;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Factories;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;
using NPOI.SS.Formula.Functions;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Models.Catalog;
using Microsoft.Identity.Client;


namespace Nop.Web.Controllers;
public class PostAdController : BasePublicController
{
    #region Fields

    protected readonly ICatalogModelFactory _catalogModelFactory;
    protected readonly IAclService _aclService;
    protected readonly IBackInStockSubscriptionService _backInStockSubscriptionService;
    protected readonly ICategoryService _categoryService;
    protected readonly ICopyProductService _copyProductService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ICustomerService _customerService;
    protected readonly IDiscountService _discountService;
    protected readonly IDownloadService _downloadService;
    protected readonly IExportManager _exportManager;
    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly IHttpClientFactory _httpClientFactory;
    protected readonly IImportManager _importManager;
    protected readonly ILanguageService _languageService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedEntityService _localizedEntityService;
    protected readonly IManufacturerService _manufacturerService;
    protected readonly INopFileProvider _fileProvider;
    protected readonly INotificationService _notificationService;
    protected readonly IPdfService _pdfService;
    protected readonly IPermissionService _permissionService;
    protected readonly IPictureService _pictureService;
    protected readonly IProductAttributeFormatter _productAttributeFormatter;
    protected readonly IProductAttributeParser _productAttributeParser;
    protected readonly IProductAttributeService _productAttributeService;
    protected readonly Areas.Admin.Factories.IProductModelFactory _productModelFactory;
    protected readonly IProductService _productService;
    protected readonly IProductTagService _productTagService;
    protected readonly ISettingService _settingService;
    protected readonly IShippingService _shippingService;
    protected readonly IShoppingCartService _shoppingCartService;
    protected readonly ISpecificationAttributeService _specificationAttributeService;
    protected readonly IStoreContext _storeContext;
    protected readonly IUrlRecordService _urlRecordService;
    protected readonly IVideoService _videoService;
    protected readonly IWebHelper _webHelper;
    protected readonly IWorkContext _workContext;
    protected readonly VendorSettings _vendorSettings;
    protected readonly ITempProductPictureService _tempProductPictureService;
    protected readonly ICategorySpecGroupService _categorySpecGroupService;
    protected readonly IRealStateLiscenceService _realStateLiscenceService;
    protected readonly Factories.IProductModelFactory _productModelFactory2;


    private readonly IHttpContextAccessor _httpContextAccessor;

    private static readonly char[] _separator = [','];

    #endregion

    #region Ctor

    public PostAdController(ICatalogModelFactory catalogModelFactory,
        IAclService aclService,
        IBackInStockSubscriptionService backInStockSubscriptionService,
        ICategoryService categoryService,
        ICopyProductService copyProductService,
        ICustomerActivityService customerActivityService,
        ICustomerService customerService,
        IDiscountService discountService,
        IDownloadService downloadService,
        IExportManager exportManager,
        IGenericAttributeService genericAttributeService,
        IHttpClientFactory httpClientFactory,
        IImportManager importManager,
        ILanguageService languageService,
        ILocalizationService localizationService,
        ILocalizedEntityService localizedEntityService,
        IManufacturerService manufacturerService,
        INopFileProvider fileProvider,
        INotificationService notificationService,
        IPdfService pdfService,
        IPermissionService permissionService,
        IPictureService pictureService,
        IProductAttributeFormatter productAttributeFormatter,
        IProductAttributeParser productAttributeParser,
        IProductAttributeService productAttributeService,
        Areas.Admin.Factories.IProductModelFactory productModelFactory,
        IProductService productService,
        IProductTagService productTagService,
        ISettingService settingService,
        IShippingService shippingService,
        IShoppingCartService shoppingCartService,
        ISpecificationAttributeService specificationAttributeService,
        IStoreContext storeContext,
        IUrlRecordService urlRecordService,
        IVideoService videoService,
        IWebHelper webHelper,
        IWorkContext workContext,
        VendorSettings vendorSettings,
        ITempProductPictureService tempProductPictureService,
        IHttpContextAccessor httpContextAccessor,
        ICategorySpecGroupService categorySpecGroupService,
        IRealStateLiscenceService realStateLiscenceService,
        Factories.IProductModelFactory productModelFactory2)
    {
        _catalogModelFactory = catalogModelFactory;
        _aclService = aclService;
        _backInStockSubscriptionService = backInStockSubscriptionService;
        _categoryService = categoryService;
        _copyProductService = copyProductService;
        _customerActivityService = customerActivityService;
        _customerService = customerService;
        _discountService = discountService;
        _downloadService = downloadService;
        _exportManager = exportManager;
        _genericAttributeService = genericAttributeService;
        _httpClientFactory = httpClientFactory;
        _importManager = importManager;
        _languageService = languageService;
        _localizationService = localizationService;
        _localizedEntityService = localizedEntityService;
        _manufacturerService = manufacturerService;
        _fileProvider = fileProvider;
        _notificationService = notificationService;
        _pdfService = pdfService;
        _permissionService = permissionService;
        _pictureService = pictureService;
        _productAttributeFormatter = productAttributeFormatter;
        _productAttributeParser = productAttributeParser;
        _productAttributeService = productAttributeService;
        _productModelFactory = productModelFactory;
        _productService = productService;
        _productTagService = productTagService;
        _settingService = settingService;
        _shippingService = shippingService;
        _shoppingCartService = shoppingCartService;
        _specificationAttributeService = specificationAttributeService;
        _storeContext = storeContext;
        _urlRecordService = urlRecordService;
        _videoService = videoService;
        _webHelper = webHelper;
        _workContext = workContext;
        _vendorSettings = vendorSettings;
        _tempProductPictureService = tempProductPictureService;
        _httpContextAccessor = httpContextAccessor;
        _categorySpecGroupService = categorySpecGroupService;
        _realStateLiscenceService = realStateLiscenceService;
        _productModelFactory2 = productModelFactory2;
    }

    #endregion

    #region Utilities




    protected virtual async Task UpdateLocalesAsync(Product product, ProductModel model)
    {
        foreach (var localized in model.Locales)
        {
            await _localizedEntityService.SaveLocalizedValueAsync(product,
                x => x.Name,
                localized.Name,
                localized.LanguageId);
            await _localizedEntityService.SaveLocalizedValueAsync(product,
                x => x.ShortDescription,
                localized.ShortDescription,
                localized.LanguageId);
            await _localizedEntityService.SaveLocalizedValueAsync(product,
                x => x.FullDescription,
                localized.FullDescription,
                localized.LanguageId);
            await _localizedEntityService.SaveLocalizedValueAsync(product,
                x => x.MetaKeywords,
                localized.MetaKeywords,
                localized.LanguageId);
            await _localizedEntityService.SaveLocalizedValueAsync(product,
                x => x.MetaDescription,
                localized.MetaDescription,
                localized.LanguageId);
            await _localizedEntityService.SaveLocalizedValueAsync(product,
                x => x.MetaTitle,
                localized.MetaTitle,
                localized.LanguageId);

            //search engine name
            var seName = await _urlRecordService.ValidateSeNameAsync(product, localized.SeName, localized.Name, false);
            await _urlRecordService.SaveSlugAsync(product, seName, localized.LanguageId);
        }
    }

    protected virtual async Task UpdateLocalesAsync(ProductTag productTag, Nop.Web.Areas.Admin.Models.Catalog.ProductTagModel model)
    {
        foreach (var localized in model.Locales)
        {
            await _localizedEntityService.SaveLocalizedValueAsync(productTag,
                x => x.Name,
                localized.Name,
                localized.LanguageId);

            var seName = await _urlRecordService.ValidateSeNameAsync(productTag, string.Empty, localized.Name, false);
            await _urlRecordService.SaveSlugAsync(productTag, seName, localized.LanguageId);
        }
    }

    protected virtual async Task UpdateLocalesAsync(ProductAttributeMapping pam, ProductAttributeMappingModel model)
    {
        foreach (var localized in model.Locales)
        {
            await _localizedEntityService.SaveLocalizedValueAsync(pam,
                x => x.TextPrompt,
                localized.TextPrompt,
                localized.LanguageId);
            await _localizedEntityService.SaveLocalizedValueAsync(pam,
                x => x.DefaultValue,
                localized.DefaultValue,
                localized.LanguageId);
        }
    }

    protected virtual async Task UpdateLocalesAsync(ProductAttributeValue pav, ProductAttributeValueModel model)
    {
        foreach (var localized in model.Locales)
        {
            await _localizedEntityService.SaveLocalizedValueAsync(pav,
                x => x.Name,
                localized.Name,
                localized.LanguageId);
        }
    }

    protected virtual async Task UpdatePictureSeoNamesAsync(Product product)
    {
        foreach (var pp in await _productService.GetProductPicturesByProductIdAsync(product.Id))
            await _pictureService.SetSeoFilenameAsync(pp.PictureId, await _pictureService.GetPictureSeNameAsync(product.Name));
    }

    protected virtual async Task SaveProductAclAsync(Product product, ProductModel model)
    {
        product.SubjectToAcl = model.SelectedCustomerRoleIds.Any();
        await _productService.UpdateProductAsync(product);

        var existingAclRecords = await _aclService.GetAclRecordsAsync(product);
        var allCustomerRoles = await _customerService.GetAllCustomerRolesAsync(true);
        foreach (var customerRole in allCustomerRoles)
        {
            if (model.SelectedCustomerRoleIds.Contains(customerRole.Id))
            {
                //new role
                if (!existingAclRecords.Any(acl => acl.CustomerRoleId == customerRole.Id))
                    await _aclService.InsertAclRecordAsync(product, customerRole.Id);
            }
            else
            {
                //remove role
                var aclRecordToDelete = existingAclRecords.FirstOrDefault(acl => acl.CustomerRoleId == customerRole.Id);
                if (aclRecordToDelete != null)
                    await _aclService.DeleteAclRecordAsync(aclRecordToDelete);
            }
        }
    }

    protected virtual async Task SaveCategoryMappingsAsync(Product product, ProductModel model)
    {
        var existingProductCategories = await _categoryService.GetProductCategoriesByProductIdAsync(product.Id, true);

        //delete categories
        foreach (var existingProductCategory in existingProductCategories)
            if (!model.SelectedCategoryIds.Contains(existingProductCategory.CategoryId))
                await _categoryService.DeleteProductCategoryAsync(existingProductCategory);

        //add categories
        foreach (var categoryId in model.SelectedCategoryIds)
        {
            if (_categoryService.FindProductCategory(existingProductCategories, product.Id, categoryId) == null)
            {
                //find next display order
                var displayOrder = 1;
                var existingCategoryMapping = await _categoryService.GetProductCategoriesByCategoryIdAsync(categoryId, showHidden: true);
                if (existingCategoryMapping.Any())
                    displayOrder = existingCategoryMapping.Max(x => x.DisplayOrder) + 1;
                await _categoryService.InsertProductCategoryAsync(new ProductCategory
                {
                    ProductId = product.Id,
                    CategoryId = categoryId,
                    DisplayOrder = displayOrder
                });
            }
        }
    }

    protected virtual async Task SaveManufacturerMappingsAsync(Product product, ProductModel model)
    {
        var existingProductManufacturers = await _manufacturerService.GetProductManufacturersByProductIdAsync(product.Id, true);

        //delete manufacturers
        foreach (var existingProductManufacturer in existingProductManufacturers)
            if (!model.SelectedManufacturerIds.Contains(existingProductManufacturer.ManufacturerId))
                await _manufacturerService.DeleteProductManufacturerAsync(existingProductManufacturer);

        //add manufacturers
        foreach (var manufacturerId in model.SelectedManufacturerIds)
        {
            if (_manufacturerService.FindProductManufacturer(existingProductManufacturers, product.Id, manufacturerId) == null)
            {
                //find next display order
                var displayOrder = 1;
                var existingManufacturerMapping = await _manufacturerService.GetProductManufacturersByManufacturerIdAsync(manufacturerId, showHidden: true);
                if (existingManufacturerMapping.Any())
                    displayOrder = existingManufacturerMapping.Max(x => x.DisplayOrder) + 1;
                await _manufacturerService.InsertProductManufacturerAsync(new ProductManufacturer
                {
                    ProductId = product.Id,
                    ManufacturerId = manufacturerId,
                    DisplayOrder = displayOrder
                });
            }
        }
    }

    protected virtual async Task SaveDiscountMappingsAsync(Product product, ProductModel model)
    {
        var allDiscounts = await _discountService.GetAllDiscountsAsync(DiscountType.AssignedToSkus, showHidden: true, isActive: null);

        foreach (var discount in allDiscounts)
        {
            if (model.SelectedDiscountIds != null && model.SelectedDiscountIds.Contains(discount.Id))
            {
                //new discount
                if (await _productService.GetDiscountAppliedToProductAsync(product.Id, discount.Id) is null)
                    await _productService.InsertDiscountProductMappingAsync(new DiscountProductMapping { EntityId = product.Id, DiscountId = discount.Id });
            }
            else
            {
                //remove discount
                if (await _productService.GetDiscountAppliedToProductAsync(product.Id, discount.Id) is DiscountProductMapping discountProductMapping)
                    await _productService.DeleteDiscountProductMappingAsync(discountProductMapping);
            }
        }

        await _productService.UpdateProductAsync(product);
        await _productService.UpdateHasDiscountsAppliedAsync(product);
    }

    protected virtual async Task<string> GetAttributesXmlForProductAttributeCombinationAsync(IFormCollection form, List<string> warnings, int productId)
    {
        var attributesXml = string.Empty;

        //get product attribute mappings (exclude non-combinable attributes)
        var attributes = (await _productAttributeService.GetProductAttributeMappingsByProductIdAsync(productId))
            .Where(productAttributeMapping => !productAttributeMapping.IsNonCombinable()).ToList();

        foreach (var attribute in attributes)
        {
            var controlId = $"{NopCatalogDefaults.ProductAttributePrefix}{attribute.Id}";
            StringValues ctrlAttributes;

            switch (attribute.AttributeControlType)
            {
                case AttributeControlType.DropdownList:
                case AttributeControlType.RadioList:
                case AttributeControlType.ColorSquares:
                case AttributeControlType.ImageSquares:
                    ctrlAttributes = form[controlId];
                    if (!string.IsNullOrEmpty(ctrlAttributes))
                    {
                        var selectedAttributeId = int.Parse(ctrlAttributes);
                        if (selectedAttributeId > 0)
                            attributesXml = _productAttributeParser.AddProductAttribute(attributesXml,
                                attribute, selectedAttributeId.ToString());
                    }

                    break;
                case AttributeControlType.Checkboxes:
                    var cblAttributes = form[controlId].ToString();
                    if (!string.IsNullOrEmpty(cblAttributes))
                    {
                        foreach (var item in cblAttributes.Split(_separator,
                                     StringSplitOptions.RemoveEmptyEntries))
                        {
                            var selectedAttributeId = int.Parse(item);
                            if (selectedAttributeId > 0)
                                attributesXml = _productAttributeParser.AddProductAttribute(attributesXml,
                                    attribute, selectedAttributeId.ToString());
                        }
                    }

                    break;
                case AttributeControlType.ReadonlyCheckboxes:
                    //load read-only (already server-side selected) values
                    var attributeValues = await _productAttributeService.GetProductAttributeValuesAsync(attribute.Id);
                    foreach (var selectedAttributeId in attributeValues
                                 .Where(v => v.IsPreSelected)
                                 .Select(v => v.Id)
                                 .ToList())
                    {
                        attributesXml = _productAttributeParser.AddProductAttribute(attributesXml,
                            attribute, selectedAttributeId.ToString());
                    }

                    break;
                case AttributeControlType.TextBox:
                case AttributeControlType.MultilineTextbox:
                    ctrlAttributes = form[controlId];
                    if (!string.IsNullOrEmpty(ctrlAttributes))
                    {
                        var enteredText = ctrlAttributes.ToString().Trim();
                        attributesXml = _productAttributeParser.AddProductAttribute(attributesXml,
                            attribute, enteredText);
                    }

                    break;
                case AttributeControlType.Datepicker:
                    var date = form[controlId + "_day"];
                    var month = form[controlId + "_month"];
                    var year = form[controlId + "_year"];
                    DateTime? selectedDate = null;
                    try
                    {
                        selectedDate = new DateTime(int.Parse(year), int.Parse(month), int.Parse(date));
                    }
                    catch
                    {
                        //ignore any exception
                    }

                    if (selectedDate.HasValue)
                    {
                        attributesXml = _productAttributeParser.AddProductAttribute(attributesXml,
                            attribute, selectedDate.Value.ToString("D"));
                    }

                    break;
                case AttributeControlType.FileUpload:
                    var requestForm = await Request.ReadFormAsync();
                    var httpPostedFile = requestForm.Files[controlId];
                    if (!string.IsNullOrEmpty(httpPostedFile?.FileName))
                    {
                        var fileSizeOk = true;
                        if (attribute.ValidationFileMaximumSize.HasValue)
                        {
                            //compare in bytes
                            var maxFileSizeBytes = attribute.ValidationFileMaximumSize.Value * 1024;
                            if (httpPostedFile.Length > maxFileSizeBytes)
                            {
                                warnings.Add(string.Format(
                                    await _localizationService.GetResourceAsync("ShoppingCart.MaximumUploadedFileSize"),
                                    attribute.ValidationFileMaximumSize.Value));
                                fileSizeOk = false;
                            }
                        }

                        if (fileSizeOk)
                        {
                            //save an uploaded file
                            var download = new Download
                            {
                                DownloadGuid = Guid.NewGuid(),
                                UseDownloadUrl = false,
                                DownloadUrl = string.Empty,
                                DownloadBinary = await _downloadService.GetDownloadBitsAsync(httpPostedFile),
                                ContentType = httpPostedFile.ContentType,
                                Filename = _fileProvider.GetFileNameWithoutExtension(httpPostedFile.FileName),
                                Extension = _fileProvider.GetFileExtension(httpPostedFile.FileName),
                                IsNew = true
                            };
                            await _downloadService.InsertDownloadAsync(download);

                            //save attribute
                            attributesXml = _productAttributeParser.AddProductAttribute(attributesXml,
                                attribute, download.DownloadGuid.ToString());
                        }
                    }

                    break;
                default:
                    break;
            }
        }

        //validate conditional attributes (if specified)
        foreach (var attribute in attributes)
        {
            var conditionMet = await _productAttributeParser.IsConditionMetAsync(attribute, attributesXml);
            if (conditionMet.HasValue && !conditionMet.Value)
            {
                attributesXml = _productAttributeParser.RemoveProductAttribute(attributesXml, attribute);
            }
        }

        return attributesXml;
    }

    protected virtual async Task SaveProductWarehouseInventoryAsync(Product product, ProductModel model)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (model.ManageInventoryMethodId != (int)ManageInventoryMethod.ManageStock)
            return;

        if (!model.UseMultipleWarehouses)
            return;

        var warehouses = await _shippingService.GetAllWarehousesAsync();

        var form = await Request.ReadFormAsync();
        var formData = form.ToDictionary(x => x.Key, x => x.Value.ToString());

        foreach (var warehouse in warehouses)
        {
            //parse stock quantity
            var stockQuantity = 0;
            foreach (var formKey in formData.Keys)
            {
                if (!formKey.Equals($"warehouse_qty_{warehouse.Id}", StringComparison.InvariantCultureIgnoreCase))
                    continue;

                _ = int.TryParse(formData[formKey], out stockQuantity);
                break;
            }

            //parse reserved quantity
            var reservedQuantity = 0;
            foreach (var formKey in formData.Keys)
                if (formKey.Equals($"warehouse_reserved_{warehouse.Id}", StringComparison.InvariantCultureIgnoreCase))
                {
                    _ = int.TryParse(formData[formKey], out reservedQuantity);
                    break;
                }

            //parse "used" field
            var used = false;
            foreach (var formKey in formData.Keys)
                if (formKey.Equals($"warehouse_used_{warehouse.Id}", StringComparison.InvariantCultureIgnoreCase))
                {
                    _ = int.TryParse(formData[formKey], out var tmp);
                    used = tmp == warehouse.Id;
                    break;
                }

            //quantity change history message
            var message = $"{await _localizationService.GetResourceAsync("Admin.StockQuantityHistory.Messages.MultipleWarehouses")} {await _localizationService.GetResourceAsync("Admin.StockQuantityHistory.Messages.Edit")}";

            var existingPwI = (await _productService.GetAllProductWarehouseInventoryRecordsAsync(product.Id)).FirstOrDefault(x => x.WarehouseId == warehouse.Id);
            if (existingPwI != null)
            {
                if (used)
                {
                    var previousStockQuantity = existingPwI.StockQuantity;

                    //update existing record
                    existingPwI.StockQuantity = stockQuantity;
                    existingPwI.ReservedQuantity = reservedQuantity;
                    await _productService.UpdateProductWarehouseInventoryAsync(existingPwI);

                    //quantity change history
                    await _productService.AddStockQuantityHistoryEntryAsync(product, existingPwI.StockQuantity - previousStockQuantity, existingPwI.StockQuantity,
                        existingPwI.WarehouseId, message);
                }
                else
                {
                    //delete. no need to store record for qty 0
                    await _productService.DeleteProductWarehouseInventoryAsync(existingPwI);

                    //quantity change history
                    await _productService.AddStockQuantityHistoryEntryAsync(product, -existingPwI.StockQuantity, 0, existingPwI.WarehouseId, message);
                }
            }
            else
            {
                if (!used)
                    continue;

                //no need to insert a record for qty 0
                existingPwI = new ProductWarehouseInventory
                {
                    WarehouseId = warehouse.Id,
                    ProductId = product.Id,
                    StockQuantity = stockQuantity,
                    ReservedQuantity = reservedQuantity
                };

                await _productService.InsertProductWarehouseInventoryAsync(existingPwI);

                //quantity change history
                await _productService.AddStockQuantityHistoryEntryAsync(product, existingPwI.StockQuantity, existingPwI.StockQuantity,
                    existingPwI.WarehouseId, message);
            }
        }
    }

    protected virtual async Task SaveConditionAttributesAsync(ProductAttributeMapping productAttributeMapping,
        ProductAttributeConditionModel model, IFormCollection form)
    {
        string attributesXml = null;
        if (model.EnableCondition)
        {
            var attribute = await _productAttributeService.GetProductAttributeMappingByIdAsync(model.SelectedProductAttributeId);
            if (attribute != null)
            {
                var controlId = $"{NopCatalogDefaults.ProductAttributePrefix}{attribute.Id}";
                switch (attribute.AttributeControlType)
                {
                    case AttributeControlType.DropdownList:
                    case AttributeControlType.RadioList:
                    case AttributeControlType.ColorSquares:
                    case AttributeControlType.ImageSquares:
                        var ctrlAttributes = form[controlId];
                        if (!StringValues.IsNullOrEmpty(ctrlAttributes))
                        {
                            var selectedAttributeId = int.Parse(ctrlAttributes);
                            //for conditions we should empty values save even when nothing is selected
                            //otherwise "attributesXml" will be empty
                            //hence we won't be able to find a selected attribute
                            attributesXml = _productAttributeParser.AddProductAttribute(null, attribute,
                                selectedAttributeId > 0 ? selectedAttributeId.ToString() : string.Empty);
                        }
                        else
                        {
                            //for conditions we should empty values save even when nothing is selected
                            //otherwise "attributesXml" will be empty
                            //hence we won't be able to find a selected attribute
                            attributesXml = _productAttributeParser.AddProductAttribute(null,
                                attribute, string.Empty);
                        }

                        break;
                    case AttributeControlType.Checkboxes:
                        var cblAttributes = form[controlId];
                        if (!StringValues.IsNullOrEmpty(cblAttributes))
                        {
                            var anyValueSelected = false;
                            foreach (var item in cblAttributes.ToString()
                                         .Split(_separator, StringSplitOptions.RemoveEmptyEntries))
                            {
                                var selectedAttributeId = int.Parse(item);
                                if (selectedAttributeId <= 0)
                                    continue;

                                attributesXml = _productAttributeParser.AddProductAttribute(attributesXml,
                                    attribute, selectedAttributeId.ToString());
                                anyValueSelected = true;
                            }

                            if (!anyValueSelected)
                            {
                                //for conditions we should save empty values even when nothing is selected
                                //otherwise "attributesXml" will be empty
                                //hence we won't be able to find a selected attribute
                                attributesXml = _productAttributeParser.AddProductAttribute(null,
                                    attribute, string.Empty);
                            }
                        }
                        else
                        {
                            //for conditions we should save empty values even when nothing is selected
                            //otherwise "attributesXml" will be empty
                            //hence we won't be able to find a selected attribute
                            attributesXml = _productAttributeParser.AddProductAttribute(null,
                                attribute, string.Empty);
                        }

                        break;
                    case AttributeControlType.ReadonlyCheckboxes:
                    case AttributeControlType.TextBox:
                    case AttributeControlType.MultilineTextbox:
                    case AttributeControlType.Datepicker:
                    case AttributeControlType.FileUpload:
                    default:
                        //these attribute types are supported as conditions
                        break;
                }
            }
        }

        productAttributeMapping.ConditionAttributeXml = attributesXml;
        await _productAttributeService.UpdateProductAttributeMappingAsync(productAttributeMapping);
    }

    protected virtual async Task GenerateAttributeCombinationsAsync(Product product, IList<int> allowedAttributeIds = null)
    {
        var allAttributesXml = await _productAttributeParser.GenerateAllCombinationsAsync(product, true, allowedAttributeIds);
        foreach (var attributesXml in allAttributesXml)
        {
            var existingCombination = await _productAttributeParser.FindProductAttributeCombinationAsync(product, attributesXml);

            //already exists?
            if (existingCombination != null)
                continue;

            //new one
            var warnings = new List<string>();
            warnings.AddRange(await _shoppingCartService.GetShoppingCartItemAttributeWarningsAsync(await _workContext.GetCurrentCustomerAsync(),
                ShoppingCartType.ShoppingCart, product, 1, attributesXml, true, true, true));
            if (warnings.Any())
                continue;

            //save combination
            var combination = new ProductAttributeCombination
            {
                ProductId = product.Id,
                AttributesXml = attributesXml,
                StockQuantity = 0,
                AllowOutOfStockOrders = false,
                Sku = null,
                ManufacturerPartNumber = null,
                Gtin = null,
                OverriddenPrice = null,
                NotifyAdminForQuantityBelow = 1
            };
            await _productAttributeService.InsertProductAttributeCombinationAsync(combination);
        }
    }

    protected virtual async Task PingVideoUrlAsync(string videoUrl)
    {
        var path = videoUrl.StartsWith("/")
            ? $"{_webHelper.GetStoreLocation()}{videoUrl.TrimStart('/')}"
            : videoUrl;

        var client = _httpClientFactory.CreateClient(NopHttpDefaults.DefaultHttpClient);
        await client.GetStringAsync(path);
    }

    protected virtual async Task SaveAttributeCombinationPicturesAsync(Product product, ProductAttributeCombination combination, ProductAttributeCombinationModel model)
    {
        var existingCombinationPictures = await _productAttributeService.GetProductAttributeCombinationPicturesAsync(combination.Id);
        var productPictureIds = (await _pictureService.GetPicturesByProductIdAsync(product.Id)).Select(p => p.Id).ToList();

        //delete manufacturers
        foreach (var existingCombinationPicture in existingCombinationPictures)
            if (!model.PictureIds.Contains(existingCombinationPicture.PictureId) || !productPictureIds.Contains(existingCombinationPicture.PictureId))
                await _productAttributeService.DeleteProductAttributeCombinationPictureAsync(existingCombinationPicture);

        //add manufacturers
        foreach (var pictureId in model.PictureIds)
        {
            if (!productPictureIds.Contains(pictureId))
                continue;

            if (_productAttributeService.FindProductAttributeCombinationPicture(existingCombinationPictures, combination.Id, pictureId) == null)
            {
                await _productAttributeService.InsertProductAttributeCombinationPictureAsync(new ProductAttributeCombinationPicture
                {
                    ProductAttributeCombinationId = combination.Id,
                    PictureId = pictureId
                });
            }
        }
    }

    protected virtual async Task SaveAttributeValuePicturesAsync(Product product, ProductAttributeValue value, ProductAttributeValueModel model)
    {
        var existingValuePictures = await _productAttributeService.GetProductAttributeValuePicturesAsync(value.Id);
        var productPictureIds = (await _pictureService.GetPicturesByProductIdAsync(product.Id)).Select(p => p.Id).ToList();

        //delete manufacturers
        foreach (var existingValuePicture in existingValuePictures)
            if (!model.PictureIds.Contains(existingValuePicture.PictureId) || !productPictureIds.Contains(existingValuePicture.PictureId))
                await _productAttributeService.DeleteProductAttributeValuePictureAsync(existingValuePicture);

        //add manufacturers
        foreach (var pictureId in model.PictureIds)
        {
            if (!productPictureIds.Contains(pictureId))
                continue;

            if (_productAttributeService.FindProductAttributeValuePicture(existingValuePictures, value.Id, pictureId) == null)
            {
                await _productAttributeService.InsertProductAttributeValuePictureAsync(new ProductAttributeValuePicture
                {
                    ProductAttributeValueId = value.Id,
                    PictureId = pictureId
                });
            }
        }
    }

    #endregion
    public async Task<IActionResult> SelectCategory()
    {
        var model = await _catalogModelFactory.PrepareTopMenuModelAsync();
        return View(model);
    }

    public async Task<IActionResult> PostNewAd(int categoryId, string liscname="", string liscid="", string liscnum="")
    {

        //validate maximum number of products per vendor
        var currentVendor = await _workContext.GetCurrentVendorAsync();
        var model = await _productModelFactory.PrepareProductModelAsync(new ProductModel(), null);

        var specificationGroup = await _categorySpecGroupService.GetGroupsByCategoryIdAsync(categoryId);
        if(specificationGroup != null)
        {
            var productSpecAttributes = await _productModelFactory2.PrepareProductSpecificationAttributeListAsync(specificationGroup.Id);
            ViewBag.productSpecAttributes = productSpecAttributes;
            model.ProductSpecsAttributes = productSpecAttributes;

        }


        //prepare model
        model.SelectedCategoryIds.Add(categoryId);
        if (!string.IsNullOrEmpty(liscname))
        {
            model.RealStateLiscence = new RealStateLiscenceModel()
            {
                HolderName = liscname,
                LiscenceNumber = liscnum,
                HolderId = liscid
            };
        }
        else
        {
            model.RealStateLiscence = null;
        }
            return View(model);
    }


    [HttpPost]
    public virtual async Task<IActionResult> PostNewAd(ProductModel model)
    {
        //if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
        //    return AccessDeniedView();

        //validate maximum number of products per vendor
        var currentVendor = (await _workContext.GetCurrentCustomerAsync()).VendorId;

        var parentCate = 31;
        var allCategories = await _categoryService.GetAllCategoriesAsync();
        var mainCategories = allCategories.Where(x => x.ParentCategoryId == 31).ToList();//GetCategoryByIdAsync(model.SelectedCategoryIds.FirstOrDefault());
        var mainCategoryIds = mainCategories.Select(x => x.Id).ToHashSet();
        var currentCategory = await _categoryService.GetCategoryByIdAsync(model.SelectedCategoryIds.FirstOrDefault());
        while (currentCategory != null && !mainCategoryIds.Contains(currentCategory.Id))
        {
            currentCategory = allCategories.FirstOrDefault(c => c.Id == currentCategory.ParentCategoryId);
        }

        parentCate = currentCategory != null ? currentCategory.Id : model.SelectedCategoryIds.FirstOrDefault();
        if (ModelState.IsValid)
        {
            //a vendor should have access only to his products
            if (currentVendor != 0)
                model.VendorId = currentVendor;

            //vendors cannot edit "Show on home page" property
            //if (currentVendor != null && model.ShowOnHomepage)
            model.ShowOnHomepage = false;

            //product
            var product = model.ToEntity<Product>();
            product.CreatedOnUtc = DateTime.UtcNow;
            product.UpdatedOnUtc = DateTime.UtcNow;
            product.Name = model.Name;
            product.AdActionId = model.AdActionId;
            product.ShowOnMaps = model.ShowOnMaps;
            product.SoldById = model.SoldById;
            product.Price = model.Price;
            product.FullDescription = model.FullDescription;
            product.Phone = model.Phone;
            product.Whatsapp = model.Whatsapp;
            product.YoutubeUrl = model.YoutubeUrl;
            product.VendorId = model.VendorId;
            product.County = model.Country;
            product.City = model.City;
            product.Coordinates = model.Coordinates;
            product.AdStatus = AdStatus.Pending;
            product.Published = false;
            product.VisibleIndividually = true;
            await _productService.InsertProductAsync(product);

            //search engine name
            model.SeName = await _urlRecordService.ValidateSeNameAsync(product, model.SeName, product.Name, true);
            await _urlRecordService.SaveSlugAsync(product, model.SeName, 0);

            //locales
            await UpdateLocalesAsync(product, model);

            //categories
            await SaveCategoryMappingsAsync(product, model);

            //manufacturers
            await SaveManufacturerMappingsAsync(product, model);

            //ACL (customer roles)
            await SaveProductAclAsync(product, model);

            //stores
            await _productService.UpdateProductStoreMappingsAsync(product, model.SelectedStoreIds);

            //discounts
            await SaveDiscountMappingsAsync(product, model);

            //tags
            await _productTagService.UpdateProductTagsAsync(product, model.SelectedProductTags.ToArray());

            //warehouses
            await SaveProductWarehouseInventoryAsync(product, model);

            //quantity change history
            await _productService.AddStockQuantityHistoryEntryAsync(product, product.StockQuantity, product.StockQuantity, product.WarehouseId,
                await _localizationService.GetResourceAsync("Admin.StockQuantityHistory.Messages.Edit"));

            //activity log
            await _customerActivityService.InsertActivityAsync("AddNewProduct",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewProduct"), product.Name), product);

            // _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Catalog.Products.Added"));

            // pictures 

            var sessionKey = GetTempImageSessionKey();
            var tempImages = await _tempProductPictureService.GetBySessionKeyAsync(sessionKey);

            int displayOrder = 0;
            foreach (var tempImg in tempImages)
            {
                var productPicture = new ProductPicture
                {
                    PictureId = tempImg.PictureId,
                    ProductId = product.Id,
                    DisplayOrder = displayOrder++
                };

                await _productService.InsertProductPictureAsync(productPicture);
            }
            displayOrder = 0;
            foreach (var spec in model.ProductSpecsAttributes)
            {
                var newSpec = new ProductSpecificationAttribute()
                {
                    ProductId = product.Id,
                    SpecificationAttributeOptionId = spec.SelectedSpecificationAttributeOptionId,
                    AllowFiltering = false,
                    ShowOnProductPage = true,
                    DisplayOrder = displayOrder++,
                    AttributeType = SpecificationAttributeType.Option,
                    AttributeTypeId = (int)SpecificationAttributeType.Option

                };
                await _specificationAttributeService.InsertProductSpecificationAttributeAsync(newSpec);
            }
            if(model.RealStateLiscence != null && !string.IsNullOrEmpty( model.RealStateLiscence.HolderName))
            {
                var liscence = new RealStateLiscence();
                liscence.ProductId = product.Id;
                liscence.CreatedOnUtc = DateTime.UtcNow;
                liscence.HolderName = model.RealStateLiscence.HolderName;
                liscence.LiscenceNumber = model.RealStateLiscence.LiscenceNumber;
                liscence.HolderId = model.RealStateLiscence.HolderId;
                await _realStateLiscenceService.AddLiscence(liscence);

            }

            // Clean up
            await _tempProductPictureService.ClearBySessionKeyAsync(sessionKey);

            return RedirectToRoute("MySubscriptions", new { adId = product.Id, catId = parentCate });
        }

        //prepare model
        model = await _productModelFactory.PrepareProductModelAsync(model, null, true);

        //if we got this far, something failed, redisplay form
        return View(model);
        // return RedirectToRoute("MySubscriptions", new { adId = 57, catId = parentCate });
    }

    public async Task<IActionResult> EditAd(int adId)
    {

        //validate maximum number of products per vendor
        var currentVendor = await _workContext.GetCurrentVendorAsync();
        //prepare model
        var ad = await _productService.GetProductByIdAsync(adId);
        var adModel = ad.ToModel<ProductModel>();
        var model = await _productModelFactory.PrepareProductModelAsync(adModel, ad);
        model.AvailableCountries.Add(new SelectListItem { Text = await _localizationService.GetResourceAsync("Address.SelectCountry"), Value = "0" });
        var specificationGroup = await _categorySpecGroupService.GetGroupsByCategoryIdAsync(model.SelectedCategoryIds.FirstOrDefault());
        if (specificationGroup != null)
        {
            var productSpecAttributes = await _productModelFactory2.PrepareProductSpecificationAttributeListAsync(specificationGroup.Id);
            ViewBag.productSpecAttributes = productSpecAttributes;
            model.ProductSpecsAttributes = productSpecAttributes;

        }

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> EditAd(ProductModel model)
    {
        //if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
        //    return AccessDeniedView();

        //try to get a product with the specified id
        var product = await _productService.GetProductByIdAsync(model.Id);
        if (product == null || product.Deleted)
            return View(model);

        ////a vendor should have access only to his products
        var currentVendor = await _workContext.GetCurrentCustomerAsync();
        //if (currentVendor != null && product.VendorId != currentVendor.VendorId)
        //    return View(model);

        //check if the product quantity has been changed while we were editing the product
        //and if it has been changed then we show error notification
        //and redirect on the editing page without data saving
        if (product.StockQuantity != model.LastStockQuantity)
        {
            _notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Admin.Catalog.Products.Fields.StockQuantity.ChangedWarning"));
            return RedirectToAction("Edit", new { id = product.Id });
        }

        if (ModelState.IsValid)
        {
            //a vendor should have access only to his products
            if (currentVendor != null)
                model.VendorId = currentVendor.VendorId;

            //we do not validate maximum number of products per vendor when editing existing products (only during creation of new products)
            //vendors cannot edit "Show on home page" property
            if (currentVendor != null && model.ShowOnHomepage != product.ShowOnHomepage)
                model.ShowOnHomepage = product.ShowOnHomepage;

            //some previously used values
            var prevTotalStockQuantity = await _productService.GetTotalStockQuantityAsync(product);
            var prevDownloadId = product.DownloadId;
            var prevSampleDownloadId = product.SampleDownloadId;
            var previousStockQuantity = product.StockQuantity;
            var previousWarehouseId = product.WarehouseId;
            var previousProductType = product.ProductType;

            //product
            product = model.ToEntity(product);

            product.UpdatedOnUtc = DateTime.UtcNow;
            product.AdStatusId = (int)AdStatus.Pending;
            await _productService.UpdateProductAsync(product);

            //remove associated products
            if (previousProductType == ProductType.GroupedProduct && product.ProductType == ProductType.SimpleProduct)
            {
                var store = await _storeContext.GetCurrentStoreAsync();
                var storeId = store?.Id ?? 0;
                var vendorId = currentVendor?.Id ?? 0;

                var associatedProducts = await _productService.GetAssociatedProductsAsync(product.Id, storeId, vendorId);
                foreach (var associatedProduct in associatedProducts)
                {
                    associatedProduct.ParentGroupedProductId = 0;
                    await _productService.UpdateProductAsync(associatedProduct);
                }
            }

            //search engine name
            model.SeName = await _urlRecordService.ValidateSeNameAsync(product, model.SeName, product.Name, true);
            await _urlRecordService.SaveSlugAsync(product, model.SeName, 0);

            //locales
            await UpdateLocalesAsync(product, model);

            //tags
            await _productTagService.UpdateProductTagsAsync(product, model.SelectedProductTags.ToArray());

            //warehouses
            await SaveProductWarehouseInventoryAsync(product, model);

            //categories
            await SaveCategoryMappingsAsync(product, model);

            //manufacturers
            await SaveManufacturerMappingsAsync(product, model);

            //ACL (customer roles)
            await SaveProductAclAsync(product, model);

            //stores
            await _productService.UpdateProductStoreMappingsAsync(product, model.SelectedStoreIds);

            //discounts
            await SaveDiscountMappingsAsync(product, model);

            var sessionKey = GetTempImageSessionKey();
            var tempImages = await _tempProductPictureService.GetBySessionKeyAsync(sessionKey);

            int displayOrder = 0;
            foreach (var tempImg in tempImages)
            {
                var productPicture = new ProductPicture
                {
                    PictureId = tempImg.PictureId,
                    ProductId = product.Id,
                    DisplayOrder = displayOrder++
                };

                await _productService.InsertProductPictureAsync(productPicture);
            }

            //picture seo names
            await UpdatePictureSeoNamesAsync(product);

            //back in stock notifications
            if (product.ManageInventoryMethod == ManageInventoryMethod.ManageStock &&
                product.BackorderMode == BackorderMode.NoBackorders &&
                product.AllowBackInStockSubscriptions &&
                await _productService.GetTotalStockQuantityAsync(product) > 0 &&
                prevTotalStockQuantity <= 0 &&
                product.Published &&
                !product.Deleted)
            {
                await _backInStockSubscriptionService.SendNotificationsToSubscribersAsync(product);
            }

            //delete an old "download" file (if deleted or updated)
            if (prevDownloadId > 0 && prevDownloadId != product.DownloadId)
            {
                var prevDownload = await _downloadService.GetDownloadByIdAsync(prevDownloadId);
                if (prevDownload != null)
                    await _downloadService.DeleteDownloadAsync(prevDownload);
            }

            //delete an old "sample download" file (if deleted or updated)
            if (prevSampleDownloadId > 0 && prevSampleDownloadId != product.SampleDownloadId)
            {
                var prevSampleDownload = await _downloadService.GetDownloadByIdAsync(prevSampleDownloadId);
                if (prevSampleDownload != null)
                    await _downloadService.DeleteDownloadAsync(prevSampleDownload);
            }

            //quantity change history
            if (previousWarehouseId != product.WarehouseId)
            {
                //warehouse is changed 
                //compose a message
                var oldWarehouseMessage = string.Empty;
                if (previousWarehouseId > 0)
                {
                    var oldWarehouse = await _shippingService.GetWarehouseByIdAsync(previousWarehouseId);
                    if (oldWarehouse != null)
                        oldWarehouseMessage = string.Format(await _localizationService.GetResourceAsync("Admin.StockQuantityHistory.Messages.EditWarehouse.Old"), oldWarehouse.Name);
                }

                var newWarehouseMessage = string.Empty;
                if (product.WarehouseId > 0)
                {
                    var newWarehouse = await _shippingService.GetWarehouseByIdAsync(product.WarehouseId);
                    if (newWarehouse != null)
                        newWarehouseMessage = string.Format(await _localizationService.GetResourceAsync("Admin.StockQuantityHistory.Messages.EditWarehouse.New"), newWarehouse.Name);
                }

                var message = string.Format(await _localizationService.GetResourceAsync("Admin.StockQuantityHistory.Messages.EditWarehouse"), oldWarehouseMessage, newWarehouseMessage);

                //record history
                await _productService.AddStockQuantityHistoryEntryAsync(product, -previousStockQuantity, 0, previousWarehouseId, message);
                await _productService.AddStockQuantityHistoryEntryAsync(product, product.StockQuantity, product.StockQuantity, product.WarehouseId, message);
            }
            else
            {
                await _productService.AddStockQuantityHistoryEntryAsync(product, product.StockQuantity - previousStockQuantity, product.StockQuantity,
                    product.WarehouseId, await _localizationService.GetResourceAsync("Admin.StockQuantityHistory.Messages.Edit"));
            }

            //activity log
            await _customerActivityService.InsertActivityAsync("EditProduct",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditProduct"), product.Name), product);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Catalog.Products.Updated"));


            return RedirectToRoute("CustomerAds");
        }

        //prepare model
        model = await _productModelFactory.PrepareProductModelAsync(model, product, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }


    [HttpPost]
    public virtual async Task<IActionResult> DeleteAd(int id)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
            return AccessDeniedView();

        //try to get a product with the specified id
        var product = await _productService.GetProductByIdAsync(id);
        await _productService.DeleteProductAsync(product);


        return Ok();
    }

    [HttpPost]
    public virtual async Task<IActionResult> PauseAd(int id)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
            return AccessDeniedView();

        //try to get a product with the specified id
        var product = await _productService.GetProductByIdAsync(id);
        product.AdStatus = AdStatus.Inactive;

        await _productService.UpdateProductAsync(product);    

        return Ok();
    }


    [HttpPost]
    public virtual async Task<IActionResult> ReActiveAd(int id)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
            return AccessDeniedView();

        //try to get a product with the specified id
        var product = await _productService.GetProductByIdAsync(id);
        product.AdStatus = AdStatus.Active;

        await _productService.UpdateProductAsync(product);

        return Ok();
    }



    public virtual IActionResult RealstateLiscence(int categoryId)
    {
        var model = new RealStateLiscenceModel();
        model.CategoryId = categoryId;
        return View(model);
    }

    [HttpPost]
    public virtual IActionResult RealstateLiscence(RealStateLiscenceModel model )
    {
        // should check here 
        return RedirectToAction("PostNewAd", new { categoryId = model.CategoryId, liscname = model.HolderName, liscid = model.HolderId, liscnum = model.LiscenceNumber });    
            
    }

    #region Product pictures

    protected virtual string GetTempImageSessionKey()
    {
        var session = _httpContextAccessor.HttpContext.Session;
        if (!session.TryGetValue("TempImageSessionKey", out _))
        {
            var newKey = Guid.NewGuid().ToString();
            session.SetString("TempImageSessionKey", newKey);
        }

        return session.GetString("TempImageSessionKey");
    }

    [HttpPost]
    public async Task<IActionResult> UploadTempProductImage(IFormFileCollection files)
    {
        if (files == null || files.Count == 0)
            return BadRequest("No files were uploaded.");

        var sessionKey = GetTempImageSessionKey();
        foreach (var file in files)
        {
            var picture = await _pictureService.InsertPictureAsync(file);

            var tempImage = new TempProductPicture
            {
                PictureId = picture.Id,
                DisplayOrder = 0, // Will be updated later
                SessionId = sessionKey,
                CreatedOnUtc = DateTime.UtcNow
            };

            await _tempProductPictureService.InsertAsync(tempImage);
        }
        return Ok();
    }




    [HttpGet]
    public async Task<IActionResult> GetTempProductImages()
    {
        var sessionKey = GetTempImageSessionKey();
        var tempImages = await _tempProductPictureService.GetBySessionKeyAsync(sessionKey);

        var result = new List<object>();
        foreach (var img in tempImages)
        {
            var pic = await _pictureService.GetPictureByIdAsync(img.PictureId);
            var url = await _pictureService.GetPictureUrlAsync(pic);

            result.Add(new
            {
                id = img.Id,
                url = url.Url,
                displayOrder = img.DisplayOrder
            });
        }

        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteTempProductImage(int id)
    {
        var tempPic = await _tempProductPictureService.GetBySessionKeyAsync(GetTempImageSessionKey());
        var picture = tempPic.FirstOrDefault(p => p.Id == id);
        if (picture == null)
            return NotFound();

        await _tempProductPictureService.DeleteAsync(picture);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> DeleteProductImage(int id,int prodId)
    {
        var product = await _productService.GetProductByIdAsync(prodId);
        var model = await _productModelFactory.PrepareProductModelAsync(null, product);
        var img = model.ProductPictureModels.FirstOrDefault(p => p.Id == id);
        await _productService.DeleteProductPictureAsync(img.ToEntity<ProductPicture>());     
        return Ok();
    }

    #endregion
}
