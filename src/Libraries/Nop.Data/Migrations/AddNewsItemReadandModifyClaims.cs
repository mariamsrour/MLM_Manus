using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Data.Migrations;
using static LinqToDB.Common.Configuration;
using System.Xml.Linq;
using FluentMigrator;
using Microsoft.EntityFrameworkCore.Migrations;
using Nop.Core.Domain.Vendors;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Nop.Core.Domain.Customers;
using Nop.Data.Extensions;
using System.Data;
using Nop.Core.Domain.Subscriptions;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.News;

namespace Nop.Data.Migrations;
[NopSchemaMigration("2025-08-15 01:00:00", "AddNewsItemReadandModifyClaims")]
public partial class AddNewsItemReadandModifyClaims : ForwardOnlyMigration
{
    public override void Up()
    {
        var table = nameof(CustomerClaims);

        if (!Schema.Table(table).Column(nameof(CustomerClaims.ModifiedBy)).Exists())
        {
            Alter.Table(table)
                .AddColumn(nameof(CustomerClaims.ModifiedBy)).AsInt32().Nullable();
        }

        if (!Schema.Table(nameof(NewsItemRead)).Exists())
        {
            Create.Table(nameof(NewsItemRead))
                .WithColumn(nameof(NewsItemRead.Id)).AsInt32().PrimaryKey().Identity()
                .WithColumn(nameof(NewsItemRead.CustomerId)).AsInt32()
                    .NotNullable().ForeignKey(nameof(Customer), nameof(Customer.Id)).OnDelete(Rule.Cascade)
                .WithColumn(nameof(NewsItemRead.NewsItemId)).AsInt32()
                    .NotNullable().ForeignKey("News", nameof(NewsItem.Id)).OnDelete(Rule.Cascade)
                .WithColumn(nameof(NewsItemRead.ReadOnUtc)).AsDateTime().NotNullable();

            // Add composite unique index (CustomerId + NewsItemId)
            Create.Index("IX_NewsItemRead_CustomerId_NewsItemId")
                .OnTable(nameof(NewsItemRead))
                .OnColumn(nameof(NewsItemRead.CustomerId)).Ascending()
                .OnColumn(nameof(NewsItemRead.NewsItemId)).Ascending()
                .WithOptions().Unique();
        }
    }

}
