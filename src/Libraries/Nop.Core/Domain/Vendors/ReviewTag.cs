using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Vendors;
public partial class ReviewTag : BaseEntity
{
    public int ReviewId { get; set; } // Links to VendorReview.Id
    public int VendorId { get; set; } // Denormalized for query efficiency
    public int TagId { get; set; } // Links to FeedbackTag.Id
}
