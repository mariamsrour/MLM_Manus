using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Common;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Common;



public partial record ClaimAdminListModel : BasePagedListModel<ClaimItems>
{
}

public partial record ClaimSearchModel : BaseSearchModel
{
    #region Ctor

    public ClaimSearchModel()
    {
        ClaimStatusOptions = new List<SelectListItem>();
       
    }

    #endregion

    #region Properties
    public List<SelectListItem> ClaimStatusOptions { get; set; }
    public string Search { get; set; }
    public int Status { get; set; }
    public DateTime? Startdate { get; set; }
    public DateTime? End { get; set; }

    #endregion
}

public partial record ClaimItems : BaseNopModel
{
    public ClaimItems()
    {
        PictureIds = new List<int>();
        PictureUrls = new List<string>();
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }
    public ClaimStatus Status { get; set; }
    public string StatusName => Status.ToString(); // or use localization
    public DateTime CreatedOnUtc { get; set; }
    public string AdminNote { get; set; }
    public int AdId { get; set; }
    public IList<int> PictureIds { get; set; }
    public IList<string> PictureUrls { get; set; }


}

public partial record ReportAdminListModel : BasePagedListModel<ReportItems>
{
}

public partial record ReportSearchModel : BaseSearchModel
{
    #region Ctor

    public ReportSearchModel()
    {
        ReportReasons = new List<SelectListItem>();
    }

    #endregion

    #region Properties
    public List<SelectListItem> ReportReasons{ get; set; }
    public string Search { get; set; }
    public int ReportReason { get; set; }
    public DateTime? Startdate { get; set; }
    public DateTime? End { get; set; }

    #endregion
}

public partial record ReportItems : BaseNopModel
{
    public ReportItems()
    {
    }
    public int Id { get; set; }
    public int ReportReason { get; set; }
    public int AdId { get; set; }
    public string Description { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string ReportReasonName { get; set; } = string.Empty; // Localized name of the report reason


}

public partial record ReportReasonListModel : BasePagedListModel<ReportReasons>
{
}

public partial record ReportReasonSearchModel : BaseSearchModel
{
    #region Ctor

    public ReportReasonSearchModel()
    {
    }

    #endregion

    #region Properties

    public string Search { get; set; }

    #endregion
}

public partial record ReportReasons : BaseNopModel
{
    public ReportReasons()
    {
        AvailableLanguages = new List<SelectListItem>();
    }
    public int Id { get; set; }
    public int LanguageId { get; set; }
    public string Language { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public List<SelectListItem> AvailableLanguages { get; set; }

    


}



