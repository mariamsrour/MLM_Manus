namespace Nop.Core.Domain.Orders;

/// <summary>
/// Represents a return status
/// </summary>
public enum ReturnRequestStatus
{
    /// <summary>
    /// Pending
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Received
    /// </summary>
    Received = 10,

    /// <summary>
    /// Return authorized
    /// </summary>
    ReturnAuthorized = 20,

    /// <summary>
    /// Item(s) repaired
    /// </summary>
    ItemsRepaired = 30,

    /// <summary>
    /// Item(s) refunded
    /// </summary>
   // ItemsRefunded = 40,

    /// <summary>
    /// Request rejected
    /// </summary>
    RequestRejected = 50,

    /// <summary>
    /// Cancelled
    /// </summary>
    Cancelled = 60,




    /// <summary>
    /// Refunded to the customer's wallet
    /// </summary>
    RefundedToWallet = 80, 

    /// <summary>
    /// Refunded to the customer's card
    /// </summary>
    RefundedToCard = 90,   

         
}


public enum ReturnToVendorStatus
{
    /// <summary>
    /// Pending
    /// </summary>
    Pending = 0,
    /// <summary>
    /// Item(s) Returned to vendor
    /// </summary>
    ItemsReturned = 40,

    /// <summary>
    /// Approved by vendor
    /// </summary>

    ApprovedByVendor = 70,

    /// <summary>
    /// Declined by vendor
    /// </summary>

    DeclinedByVendor = 80,

    /// <summary>
    /// Vendor claimed the return
    /// </summary>
    Claimed = 100
}