using Nop.Core.Domain.Catalog;
using Nop.Web.Framework.Models;
using Nop.Web.Models.Media;
using Nop.Web.Models.Vendors;

namespace Nop.Web.Models.Catalog;

public partial record ProductOverviewModel : BaseNopEntityModel
{
    public ProductOverviewModel()
    {
        ProductPrice = new ProductPriceModel();
        PictureModels = new List<PictureModel>();
        ProductSpecificationModel = new ProductSpecificationModel();
        ReviewOverviewModel = new ProductReviewOverviewModel();
        ProductTags = new List<ProductTagModel>();
        ProductAttributes = new List<ProductDetailsModel.ProductAttributeModel>();
        VendorModel = new VendorInfoModel();


    }

    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public string FullDescription { get; set; }
    public string SeName { get; set; }

    public string Sku { get; set; }

    public ProductType ProductType { get; set; }

    public bool MarkAsNew { get; set; }

    public DateTime CreatedOn { get; set; }

    public string City { get; set; }

    public string Country { get; set; }

    public AdStatus AdStatus { get; set; }

    public string Category { get; set; }

    public string RejectionReason { get; set; }


    //price
    public ProductPriceModel ProductPrice { get; set; }
    //pictures
    public IList<PictureModel> PictureModels { get; set; }
    //specification attributes
    public ProductSpecificationModel ProductSpecificationModel { get; set; }
    //price
    public ProductReviewOverviewModel ReviewOverviewModel { get; set; }

    public IList<ProductTagModel> ProductTags { get; set; }

    public IList<ProductDetailsModel.ProductAttributeModel> ProductAttributes { get; set; }

    public VendorInfoModel VendorModel { get; set; }

    #region Nested Classes

    public partial record ProductPriceModel : BaseNopModel
    {
        public string OldPrice { get; set; }
        public decimal? OldPriceValue { get; set; }
        public string Price { get; set; }
        public decimal? PriceValue { get; set; }
        /// <summary>
        /// PAngV baseprice (used in Germany)
        /// </summary>
        public string BasePricePAngV { get; set; }
        public decimal? BasePricePAngVValue { get; set; }

        public bool DisableBuyButton { get; set; }
        public bool DisableWishlistButton { get; set; }
        public bool DisableAddToCompareListButton { get; set; }

        public bool AvailableForPreOrder { get; set; }
        public DateTime? PreOrderAvailabilityStartDateTimeUtc { get; set; }

        public bool IsRental { get; set; }

        public bool ForceRedirectionAfterAddingToCart { get; set; }

        /// <summary>
        /// A value indicating whether we should display tax/shipping info (used in Germany)
        /// </summary>
        public bool DisplayTaxShippingInfo { get; set; }

    }

    #endregion
}