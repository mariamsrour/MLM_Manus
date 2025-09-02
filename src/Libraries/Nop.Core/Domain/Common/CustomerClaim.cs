using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Media;

namespace Nop.Core.Domain.Common;
[Table("CustomerClaims")]
public class CustomerClaims: BaseEntity
{
    public string Title { get; set; }
    public string Username { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }
    public string AdminNote { get; set; }
    public int AdId { get; set; }
    public int CustomerId { get; set; }
    public int Status { get; set; }
    public string AttachemntIds { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public int ModifiedBy { get; set; }
}


public enum ClaimStatus
{
    Opened = 1,
    Closed = 2
}

