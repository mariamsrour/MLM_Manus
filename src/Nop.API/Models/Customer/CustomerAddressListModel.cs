using Nop.Web.Framework.Models;
using Nop.API.Models.Common;

namespace Nop.API.Models.Customer;

public partial record CustomerAddressListModel : BaseNopModel
{
    public CustomerAddressListModel()
    {
        Addresses = new List<AddressModel>();
    }

    public IList<AddressModel> Addresses { get; set; }
}