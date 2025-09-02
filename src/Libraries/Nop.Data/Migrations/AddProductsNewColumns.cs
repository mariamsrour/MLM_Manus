using System.Data;
using FluentMigrator;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Orders;
using Nop.Data.Extensions;
using Nop.Core.Domain.Vendors;


namespace Nop.Data.Migrations
{
    [NopSchemaMigration("2025-05-12 01:00:00", "AddProductsNewColumns")]

    public partial class AddProductsNewColumns : ForwardOnlyMigration
    {
        public override void Up()
        {
            // Product table updates
            var customerTable = nameof(Product);

            if (!Schema.Table(customerTable).Column(nameof(Product.AdActionId)).Exists())
                Alter.Table(customerTable)
                    .AddColumn(nameof(Product.AdActionId)).AsInt32().Nullable().WithDefaultValue(0);

            if (!Schema.Table(customerTable).Column(nameof(Product.SoldById)).Exists())
                Alter.Table(customerTable)
                    .AddColumn(nameof(Product.SoldById)).AsInt32().Nullable().WithDefaultValue(0);

            if (!Schema.Table(customerTable).Column(nameof(Product.ShowOnMaps)).Exists())
                Alter.Table(customerTable)
                    .AddColumn(nameof(Product.ShowOnMaps)).AsBoolean().Nullable().WithDefaultValue(false);

            if (!Schema.Table(customerTable).Column(nameof(Product.Phone)).Exists())
                Alter.Table(customerTable)
                    .AddColumn(nameof(Product.Phone)).AsString().Nullable();

            if (!Schema.Table(customerTable).Column(nameof(Product.Whatsapp)).Exists())
                Alter.Table(customerTable)
                    .AddColumn(nameof(Product.Whatsapp)).AsString().Nullable();

            if (!Schema.Table(customerTable).Column(nameof(Product.YoutubeUrl)).Exists())
                Alter.Table(customerTable)
                    .AddColumn(nameof(Product.YoutubeUrl)).AsString().Nullable();
        }

    }
}