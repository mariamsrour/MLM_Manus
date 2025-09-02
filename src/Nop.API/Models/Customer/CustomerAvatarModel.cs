using Nop.Web.Framework.Models;

namespace Nop.API.Models.Customer;

public partial record CustomerAvatarModel : BaseNopModel
{
    public string AvatarUrl { get; set; }
}