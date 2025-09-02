using System.Data;
using FluentMigrator;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Data.Extensions;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Vendors;
using Nop.Core.Domain.Subscriptions;


namespace Nop.Data.Migrations
{
    [NopSchemaMigration("2025-04-30 01:00:00", "AddSubscriptionCategory")]

    public partial class AddSubscriptionCategory : ForwardOnlyMigration
    {
        public override void Up()
        {
            // Product table updates
            var customerTable = nameof(UserSubscription);

            if (!Schema.Table(customerTable).Column(nameof(UserSubscription.CategoryId)).Exists())
                Alter.Table(customerTable)
                    .AddColumn(nameof(UserSubscription.CategoryId)).AsInt32().NotNullable().WithDefaultValue(0);

        
        }

    }
}