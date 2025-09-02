namespace Nop.Core.Domain.Catalog;

/// <summary>
/// Represents a product type
/// </summary>
public enum ProductType
{
    /// <summary>
    /// Simple
    /// </summary>
    SimpleProduct = 5,

    /// <summary>
    /// Grouped (product with variants)
    /// </summary>
    GroupedProduct = 10,


    ForSale = 11,

    ForRent = 12,

    BuyRequest = 13,
}

public enum AdAction
{
    Price = 100,
    PleaseContact = 101,
}

public enum SoldBy
{
    Owner = 200,
    Business = 201,
}

public enum AdStatus
{
    Active = 300,
    Inactive = 301,
    Pending = 302,
    Rejected = 303,
}
