using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Models.Vendors;

public partial record VendorInfoModel : BaseNopModel
{
    public VendorInfoModel()
    {
        VendorAttributes = new List<VendorAttributeModel>();
        VendorReviews = new List<VendorReviewModel>();
    }

    public int VendorId { get; set; }

    [NopResourceDisplayName("Account.VendorInfo.Name")]
    public string Name { get; set; }

    [DataType(DataType.EmailAddress)]
    [NopResourceDisplayName("Account.VendorInfo.Email")]
    public string Email { get; set; }

    [NopResourceDisplayName("Account.VendorInfo.Description")]
    public string Description { get; set; }

    [NopResourceDisplayName("Account.VendorInfo.Picture")]
    public string PictureUrl { get; set; }

    public decimal ApprovedRatingSum { get; set; }
    public int NotApprovedRatingSum { get; set; }
    public int ApprovedTotalReviews { get; set; }
    public int NotApprovedTotalReviews { get; set; }

    public string Phone { get; set; }

    public string WhatsappLink { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public string AvgReply { get; set; }

    public string AvgReplyRate { get; set; }

    public string SeName { get; set; }

    


    public IList<VendorAttributeModel> VendorAttributes { get; set; }

    public IList<VendorReviewModel> VendorReviews { get; set; }

    public int VendorProductsCount { get; set; }



}