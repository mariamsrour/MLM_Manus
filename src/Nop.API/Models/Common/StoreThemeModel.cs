using Nop.Web.Framework.Models;

namespace Nop.API.Models.Common;

public partial record StoreThemeModel : BaseNopModel
{
    public string Name { get; set; }
    public string Title { get; set; }
}