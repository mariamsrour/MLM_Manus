using Nop.Web.Framework.Models;
using Nop.Web.Models.Media;
using Nop.Web.Models.Vendors;

namespace Nop.Web.Models.Catalog;

public partial record VendorModel : BaseNopEntityModel
{
    public VendorModel()
    {
        PictureModel = new PictureModel();
        CatalogProductsModel = new CatalogProductsModel();
        VendorReviews = new List<VendorReviewModel>();
        VendorTopTags = new List<VendorTopTag>();
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public string MetaKeywords { get; set; }
    public string MetaDescription { get; set; }
    public string MetaTitle { get; set; }
    public string SeName { get; set; }
    public bool AllowCustomersToContactVendors { get; set; }

    public PictureModel PictureModel { get; set; }

    public CatalogProductsModel CatalogProductsModel { get; set; }

    public decimal ApprovedRatingSum { get; set; }
    public int NotApprovedRatingSum { get; set; }
    public int ApprovedTotalReviews { get; set; }
    public int NotApprovedTotalReviews { get; set; }

    public string Phone { get; set; }

    public string WhatsappLink { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public string AvgReply { get; set; }

    public string AvgReplyRate { get; set; }

    public IList<VendorReviewModel> VendorReviews { get; set; }

    public IList<VendorTopTag> VendorTopTags { get; set; }


}


public partial record VendorTopTag : BaseNopEntityModel
{
    public  VendorTopTag() { }
    public string Tag { get; set; }
    public int Count { get; set; }  
}

