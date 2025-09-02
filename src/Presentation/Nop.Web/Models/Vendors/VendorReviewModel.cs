using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Models.Catalog;

namespace Nop.Web.Models.Vendors;

public partial record VendorReviewModel : BaseNopModel
{
    public VendorReviewModel()
    {
        AvailableTags = new List<VendorTopTag>();
        SelectedTagIds = new List<int>();
        Tags = new List<string>();  
    }
    public string Title { get; set; }
    public string ReviewText { get; set; }
    public int Rating { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public string CustomerName { get; set; }
    public string CustomerPicture { get; set; }
    public List<string> Tags { get; set; }
    public List<VendorTopTag> AvailableTags { get; set; }
    public List<int> SelectedTagIds { get; set; }

    public int VendorId { get; set; }
}
