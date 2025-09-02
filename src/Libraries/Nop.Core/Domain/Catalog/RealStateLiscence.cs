using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Catalog;
public partial class RealStateLiscence: BaseEntity
{
    public string  HolderName { get; set; }
    public string HolderId { get; set; }
    public string LiscenceNumber { get; set; }
    public int ProductId { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public virtual Product Product { get; set; }


}
