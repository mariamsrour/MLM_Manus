﻿using Nop.Web.Framework.Models;

namespace Nop.API.Models.Checkout;

public partial record CheckoutPaymentInfoModel : BaseNopModel
{
    public Type PaymentViewComponent { get; set; }

    /// <summary>
    /// Used on one-page checkout page
    /// </summary>
    public bool DisplayOrderTotals { get; set; }
}