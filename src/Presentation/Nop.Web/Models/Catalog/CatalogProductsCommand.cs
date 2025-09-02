using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.UI.Paging;

namespace Nop.Web.Models.Catalog;

/// <summary>
/// Represents a model to get the catalog products
/// </summary>
public partial record CatalogProductsCommand : BasePageableModel
{
    #region Properties

    /// <summary>
    /// Gets or sets the price ('min-max' format)
    /// </summary>
    public string Price { get; set; }

    /// <summary>
    /// Gets or sets the specification attribute option ids
    /// </summary>
    [FromQuery(Name = "specs")]
    public List<int> SpecificationOptionIds { get; set; }

    /// <summary>
    /// Gets or sets the manufacturer ids
    /// </summary>
    [FromQuery(Name = "ms")]
    public List<int> ManufacturerIds { get; set; }

    /// <summary>
    /// Gets or sets a order by
    /// </summary>
    public int? OrderBy { get; set; }

    /// <summary>
    /// Gets or sets a product sorting
    /// </summary>
    public string ViewMode { get; set; }


    [FromQuery(Name = "type")]
    public int? ProductType { get; set; }

    [FromQuery(Name = "city")]
    public string City { get; set; }

    [FromQuery(Name = "country")]
    public string Country { get; set; }

    public bool? All { get; set; }
    #endregion
}