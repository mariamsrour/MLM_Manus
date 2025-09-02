using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace Nop.API.Models.Subscriptions; 
public partial record UserSubscription : BaseNopModel
{
    public UserSubscription()
    {
        Package = new Package();
    }
    public int Id { get; set; }

    public int CustomerId { get; set; }
    public int PackageId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public int HighlightedUsed { get; set; }
    public int NormalUsed { get; set; }
    public bool IsActive { get; set; }
    public decimal PaidPrice { get; set; }
    public Package Package { get; set; }
}
