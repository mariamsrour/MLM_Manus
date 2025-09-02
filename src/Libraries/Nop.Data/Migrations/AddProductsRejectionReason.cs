using System.Data;
using FluentMigrator;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Orders;
using Nop.Data.Extensions;
using Nop.Core.Domain.Vendors;


namespace Nop.Data.Migrations
{
    [NopSchemaMigration("2025-05-30 02:00:00", "AddProductsRejectionReason")]

    public partial class AddProductsRejectionReason : ForwardOnlyMigration
    {
        public override void Up()
        {
            // Product table updates
            var customerTable = nameof(Product);

            if (!Schema.Table(customerTable).Column(nameof(Product.RejectionReason)).Exists())
                Alter.Table(customerTable)
                    .AddColumn(nameof(Product.RejectionReason)).AsString().Nullable();
        }
    }
}