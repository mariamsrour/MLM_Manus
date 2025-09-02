using Nop.Web.Framework.Models;

namespace Nop.API.Models.Newsletter;

public partial record SubscriptionActivationModel : BaseNopModel
{
    public string Result { get; set; }
}