using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Vendors
{
    public partial class FeedbackTag : BaseEntity
    {
        public string Tag { get; set; } // E.g., "Fast Delivery", "Great Service"
    }
}
