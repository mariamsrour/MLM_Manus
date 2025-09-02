using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace Nop.Web.Models.Subscriptions;
public partial record Plan : BaseNopModel
{
    public Plan()
    {
        Features = new List<string>();
        Packages = new List<Package>();
    }
    public int Id { get; set; }
    public string Name { get; set; } // e.g., "Basic", "Pro"
    public string Description { get; set; }
    public List<string> Features { get; set; } // JSON: ["24/7 Support", "3 Markets"]
    public bool IsPopular { get; set; }
    public virtual ICollection<Package> Packages { get; set; }
}