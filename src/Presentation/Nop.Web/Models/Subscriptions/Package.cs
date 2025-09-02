using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace Nop.Web.Models.Subscriptions;
public partial record Package : BaseNopModel
{
    public Package()
    {
        Plan = new Plan();
    }
    public int Id { get; set; }
    public int PlanId { get; set; }
    public string Name { get; set; } // e.g., "Highlight", "Normal"
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
    public int HighlightedAds { get; set; }
    public int NormalAds { get; set; }

    public Plan Plan { get; set; }
}
