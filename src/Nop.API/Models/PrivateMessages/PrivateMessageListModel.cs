using Nop.Web.Framework.Models;
using Nop.API.Models.Common;

namespace Nop.API.Models.PrivateMessages;

public partial record PrivateMessageListModel : BaseNopModel
{
    public IList<PrivateMessageModel> Messages { get; set; }
    public PagerModel PagerModel { get; set; }
}