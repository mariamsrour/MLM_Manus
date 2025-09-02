using System.Data;
using FluentMigrator;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Data.Extensions;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Vendors;


namespace Nop.Data.Migrations
{
    [NopSchemaMigration("2025-04-03 01:00:00", "Add product, specification, and vendor fields")]

    public partial class AddCustomFieldsMigration : ForwardOnlyMigration
    {
        public override void Up()
        {
            // Product table updates
            var productTable = nameof(Product);

            if (!Schema.Table(productTable).Column(nameof(Product.County)).Exists())
                Alter.Table(productTable)
                    .AddColumn(nameof(Product.County)).AsString(400).Nullable();

            if (!Schema.Table(productTable).Column(nameof(Product.City)).Exists())
                Alter.Table(productTable)
                    .AddColumn(nameof(Product.City)).AsString(400).Nullable();

            if (!Schema.Table(productTable).Column(nameof(Product.Coordinates)).Exists())
                Alter.Table(productTable)
                    .AddColumn(nameof(Product.Coordinates)).AsString(400).Nullable();

            // ProductSpecificationAttribute table updates
            var specTable = nameof(SpecificationAttribute);
            if (!Schema.Table(specTable).Column(nameof(SpecificationAttribute.Icon)).Exists())
                Alter.Table(specTable)
                    .AddColumn(nameof(SpecificationAttribute.Icon)).AsString(400).Nullable();

            // Vendor table updates
            var vendorTable = nameof(Vendor);

            if (!Schema.Table(vendorTable).Column(nameof(Vendor.Phone)).Exists())
                Alter.Table(vendorTable)
                    .AddColumn(nameof(Vendor.Phone)).AsString(50).Nullable();

            if (!Schema.Table(vendorTable).Column(nameof(Vendor.WhatsappLink)).Exists())
                Alter.Table(vendorTable)
                    .AddColumn(nameof(Vendor.WhatsappLink)).AsString(400).Nullable();

            if (!Schema.Table(vendorTable).Column(nameof(Vendor.CreatedOnUtc)).Exists())
                Alter.Table(vendorTable)
                    .AddColumn(nameof(Vendor.CreatedOnUtc)).AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime);

            if (!Schema.Table(vendorTable).Column(nameof(Vendor.AvgReply)).Exists())
                Alter.Table(vendorTable)
                    .AddColumn(nameof(Vendor.AvgReply)).AsString(50).Nullable();

            if (!Schema.Table(vendorTable).Column(nameof(Vendor.AvgReplyRate)).Exists())
                Alter.Table(vendorTable)
                    .AddColumn(nameof(Vendor.AvgReplyRate)).AsString(50).Nullable();
        }
       
    }
}