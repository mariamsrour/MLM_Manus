using Nop.Web.Framework.Models;

namespace Nop.Web.Models.Catalog;

public partial record RealStateLiscenceModel : BaseNopEntityModel
{

    public string HolderName { get; set; }
    public string HolderId { get; set; }
    public string LiscenceNumber { get; set; }
    public int ProductId { get; set; }
    public int CategoryId { get; set; }

    public DateTime CreatedOnUtc { get; set; }
}
