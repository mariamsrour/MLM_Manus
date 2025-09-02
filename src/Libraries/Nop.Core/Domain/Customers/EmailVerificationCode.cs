using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Customers;
public class EmailVerificationCode : BaseEntity
{
    public int CustomerId { get; set; }
    public string Code { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public bool IsUsed { get; set; }

    public virtual Customer Customer { get; set; }
}

