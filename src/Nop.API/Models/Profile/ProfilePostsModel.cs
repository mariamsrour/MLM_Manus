using Nop.Web.Framework.Models;
using Nop.API.Models.Common;

namespace Nop.API.Models.Profile;

public partial record ProfilePostsModel : BaseNopModel
{
    public IList<PostsModel> Posts { get; set; }
    public PagerModel PagerModel { get; set; }
}