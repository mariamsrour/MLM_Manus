using Nop.Web.Framework.Models;

namespace Nop.API.Models.Common;

public partial record FaviconAndAppIconsModel : BaseNopModel
{
    public string HeadCode { get; set; }
}