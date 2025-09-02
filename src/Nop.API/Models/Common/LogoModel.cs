using Nop.Web.Framework.Models;

namespace Nop.API.Models.Common;

public partial record LogoModel : BaseNopModel
{
    public string StoreName { get; set; }

    public string LogoPath { get; set; }
}