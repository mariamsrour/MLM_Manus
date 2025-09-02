using Nop.Web.Framework.Models;

namespace Nop.API.Models.Common;

public partial record CurrencyModel : BaseNopEntityModel
{
    public string Name { get; set; }

    public string CurrencySymbol { get; set; }
}