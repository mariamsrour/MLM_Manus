namespace Nop.API.Models.Checkout;

public partial record UpdateSectionJsonModel
{
    public string name { get; set; }
    public string html { get; set; }
}