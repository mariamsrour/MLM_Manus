using Nop.Web.Framework.Models;

namespace Nop.API.Models.Sitemap;

public partial record SitemapXmlModel : BaseNopModel
{
    public string SitemapXmlPath { get; set; }
}