using Nop.Web.Framework.Models;

namespace Nop.API.Models.Customer;

public partial record AccountActivationModel : BaseNopModel
{
    public string Result { get; set; }

    public string ReturnUrl { get; set; }
}