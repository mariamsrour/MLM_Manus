using Nop.Web.Framework.Models;

namespace Nop.API.Models.Blogs;

public partial record BlogPostTagModel : BaseNopModel
{
    public string Name { get; set; }

    public int BlogPostCount { get; set; }
}