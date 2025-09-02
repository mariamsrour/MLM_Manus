using System.ComponentModel.DataAnnotations;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Wordprocessing;
using Nop.Core.Domain.Common;
using Nop.Web.Framework.Models;

namespace Nop.Web.Models.Common;

public partial record ClaimModel : BaseNopEntityModel
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }
    [Required]
    public int AdId { get; set; }
    public IList<IFormFile> Attachments { get; set; }

    public CustomerClaims ToEntity(CustomerClaims destination = null)
    {
        destination ??= new CustomerClaims();
        destination.Id = Id;
        destination.Title = Title;
        destination.Username = Name;
        destination.PhoneNumber = PhoneNumber;
        destination.Email = Email;
        destination.Message = Message;
        destination.AdId = AdId;
        destination.Status = (int) ClaimStatus.Opened;
        destination.CreatedOnUtc = DateTime.UtcNow;
        return destination;
    }

}
