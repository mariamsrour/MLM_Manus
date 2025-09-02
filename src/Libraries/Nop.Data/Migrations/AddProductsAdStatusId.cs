using System.Data;
using FluentMigrator;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Orders;
using Nop.Data.Extensions;
using Nop.Core.Domain.Vendors;


namespace Nop.Data.Migrations
{
    [NopSchemaMigration("2025-05-12 02:00:00", "AddProductsAdStatusId")]

    public partial class AddProductsAdStatusId : ForwardOnlyMigration
    {
        public override void Up()
        {
            // Product table updates
            var customerTable = nameof(Product);

            if (!Schema.Table(customerTable).Column(nameof(Product.AdStatusId)).Exists())
                Alter.Table(customerTable)
                    .AddColumn(nameof(Product.AdStatusId)).AsInt32().Nullable().WithDefaultValue(0);


        }
    }
}