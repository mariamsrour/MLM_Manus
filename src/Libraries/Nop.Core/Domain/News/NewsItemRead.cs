using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Customers;

namespace Nop.Core.Domain.News;
public class NewsItemRead : BaseEntity
{
    public int NewsItemId { get; set; }
    public int CustomerId { get; set; }
    public DateTime ReadOnUtc { get; set; }

    // Navigation props
    public virtual NewsItem NewsItem { get; set; }
    public virtual Customer Customer { get; set; }
}
