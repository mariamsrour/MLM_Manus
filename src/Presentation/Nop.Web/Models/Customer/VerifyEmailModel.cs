using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;

namespace Nop.Web.Models.Customer;

public partial record VerifyEmailModel: BaseNopModel
{
    [Required]
    public string Code { get; set; }

    public int CustomerId { get; set; }
}
