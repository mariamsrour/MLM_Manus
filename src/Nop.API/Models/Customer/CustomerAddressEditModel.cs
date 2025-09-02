using Nop.Web.Framework.Models;
using Nop.API.Models.Common;

namespace Nop.API.Models.Customer;

public partial record CustomerAddressEditModel : BaseNopModel
{
    public CustomerAddressEditModel()
    {
        Address = new AddressModel();
    }

    public AddressModel Address { get; set; }
}