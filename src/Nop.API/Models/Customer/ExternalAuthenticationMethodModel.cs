using Nop.Web.Framework.Models;

namespace Nop.API.Models.Customer;

public partial record ExternalAuthenticationMethodModel : BaseNopModel
{
    public Type ViewComponent { get; set; }
}