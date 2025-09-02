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
    [NopSchemaMigration("2025-04-25 01:00:00", "AddCustomerFlags")]

    public partial class AddCustomerFlags : ForwardOnlyMigration
    {
        public override void Up()
        {
            // Product table updates
            var customerTable = nameof(Customer);

            if (!Schema.Table(customerTable).Column(nameof(Customer.HideVisits)).Exists())
                Alter.Table(customerTable)
                    .AddColumn(nameof(Customer.HideVisits)).AsBoolean().Nullable().WithDefaultValue(false);

            if (!Schema.Table(customerTable).Column(nameof(Customer.SendEmails)).Exists())
                Alter.Table(customerTable)
                    .AddColumn(nameof(Customer.SendEmails)).AsBoolean().Nullable().WithDefaultValue(false);

            if (!Schema.Table(customerTable).Column(nameof(Customer.ContactMe)).Exists())
                Alter.Table(customerTable)
                    .AddColumn(nameof(Customer.ContactMe)).AsBoolean().Nullable().WithDefaultValue(false);



        }

    }
}