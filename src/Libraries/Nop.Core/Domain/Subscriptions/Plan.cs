using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Subscriptions;
public class Plan : BaseEntity
{
    public Plan()
    {
        Packages = new List<Package>();
    }
    public string Name { get; set; } // e.g., "Basic", "Pro"
    public string Description { get; set; }
    public string Features { get; set; } // JSON: ["24/7 Support", "3 Markets"]
    public bool IsPopular { get; set; }
    public virtual ICollection<Package> Packages { get; set; }
}