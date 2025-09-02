using System.Data;
using FluentMigrator;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Data.Extensions;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Vendors;
using Nop.Core.Domain.News;


namespace Nop.Data.Migrations
{
    [NopSchemaMigration("2025-07-16 01:00:00", "AddPrivateNotification")]

    public partial class AddPrivateNotification : ForwardOnlyMigration
    {
        public override void Up()
        {
            // Product table updates
           // var customerTable = nameof(NewsItem);

            if (!Schema.Table("News").Column(nameof(NewsItem.CustomerId)).Exists())
                Alter.Table("News")
                    .AddColumn(nameof(NewsItem.CustomerId)).AsInt32().Nullable();
        }

    }
}