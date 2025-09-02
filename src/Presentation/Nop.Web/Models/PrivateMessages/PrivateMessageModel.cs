using Nop.Core.Domain.Customers;
using Nop.Web.Framework.Models;


namespace Nop.Web.Models.PrivateMessages;

public partial record PrivateMessageModel : BaseNopEntityModel
{
    public int FromCustomerId { get; set; }
    public string CustomerFromName { get; set; }
    public bool AllowViewingFromProfile { get; set; }

    public int ToCustomerId { get; set; }
    public string CustomerToName { get; set; }
    public bool AllowViewingToProfile { get; set; }

    public string Subject { get; set; }

    public string Message { get; set; }

    public DateTime CreatedOn { get; set; }

    public bool IsRead { get; set; }

    public int AdId { get; set; }
    public int AdPictureId { get; set; }
    public string ToCustomerPictureUrl { get; set; }
    public string FromCustomerPictureUrl { get; set; }
    public string AdPictureUrl { get; set; }
}

public partial record PrivateMessageThreadModel : BaseNopEntityModel
{
    public PrivateMessageThreadModel ()
    {
        Messages = new List<PrivateMessageModel>();
}

    public int FromCustomerId { get; set; }
    public string CustomerFromName { get; set; }

    public int ToCustomerId { get; set; }
    public string CustomerToName { get; set; }
    public string Subject { get; set; }

    public int AdId { get; set; }
    public int AdPictureId { get; set; }

    public string ToCustomerPictureUrl { get; set; }
    public string FromCustomerPictureUrl { get; set; }
    public string AdPictureUrl { get; set; }

    public List<PrivateMessageModel> Messages { get; set; }
}