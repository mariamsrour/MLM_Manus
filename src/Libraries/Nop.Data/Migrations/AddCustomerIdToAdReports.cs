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
    [NopSchemaMigration("2025-07-20 01:00:00", "AddCustomerIdToAdReports")]

    public partial class AddCustomerIdToAdReportsMigration : ForwardOnlyMigration
    {
        public override void Up()
        {
            // Product table updates
            var productTable = "AdReports";

            if (!Schema.Table(productTable).Column(nameof(AdReports.CustomerId)).Exists())
                Alter.Table(productTable).AddColumn(nameof(AdReports.CustomerId)).AsInt32().Nullable().ForeignKey<Customer>();

        }
       
    }
}