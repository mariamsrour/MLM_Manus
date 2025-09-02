using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Vendors;
using Nop.Core;

public partial class VendorReview : BaseEntity
{
    public int VendorId { get; set; }
    public int CustomerId { get; set; }
    public bool IsApproved { get; set; }
    public string Title { get; set; }
    public string ReviewText { get; set; }
    public int Rating { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public virtual Vendor Vendor { get; set; }
    public virtual Customer Customer { get; set; }
}