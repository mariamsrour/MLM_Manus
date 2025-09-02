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
[NopSchemaMigration("2025-04-11 00:00:00", "AddPackageTable")]
public partial class AddPackageTable : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Package")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("PlanId").AsInt32().NotNullable()
                .ForeignKey("Plan", "Id")
            .WithColumn("Name").AsString(100).NotNullable() // e.g., "Highlight", "Normal"
            .WithColumn("Price").AsDecimal(18, 4).NotNullable()
            .WithColumn("DurationDays").AsInt32().NotNullable()
            .WithColumn("HighlightedAds").AsInt32().NotNullable()
            .WithColumn("NormalAds").AsInt32().NotNullable();

    }

}
