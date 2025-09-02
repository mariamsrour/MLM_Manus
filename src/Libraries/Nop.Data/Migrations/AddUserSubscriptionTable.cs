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

namespace Nop.Data.Migrations;
[NopSchemaMigration("2025-04-12 00:00:00", "AddUserSubscription")]
public partial class AddUserSubscription : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("UserSubscription")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("CustomerId").AsInt32().NotNullable()
                .ForeignKey("Customer", "Id")
            .WithColumn("PackageId").AsInt32().NotNullable()
                .ForeignKey("Package", "Id")
            .WithColumn("StartDate").AsDateTime().NotNullable()
            .WithColumn("ExpirationDate").AsDateTime().NotNullable()
            .WithColumn("HighlightedUsed").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("NormalUsed").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);
    }

}
