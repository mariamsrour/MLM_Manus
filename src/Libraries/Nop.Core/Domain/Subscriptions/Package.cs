using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Subscriptions;
public class Package : BaseEntity
{
    public int PlanId { get; set; }
    public string Name { get; set; } // e.g., "Highlight", "Normal"
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
    public int HighlightedAds { get; set; }
    public int NormalAds { get; set; }
    public virtual Plan Plan { get; set; }
}
