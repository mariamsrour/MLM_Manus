using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Subscriptions;
public class UserSubscription : BaseEntity
{
    public int CustomerId { get; set; }
    public int PackageId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public int HighlightedUsed { get; set; }
    public int NormalUsed { get; set; }
    public bool IsActive { get; set; }

    [Column("PaidPrice")] 
    public decimal PaidPrice { get; set; }
    public virtual Package Package { get; set; }
    public int CategoryId { get; set; }
}
