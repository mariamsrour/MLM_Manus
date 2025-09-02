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
[NopSchemaMigration("2025-04-10 00:00:00", "Plan")]
public partial class AddPlanTable : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Plan")
              .WithColumn("Id").AsInt32().PrimaryKey().Identity()
              .WithColumn("Name").AsString(100).NotNullable() // e.g., "Basic", "Pro"
              .WithColumn("Description").AsString(500).NotNullable()
              .WithColumn("Features").AsString(int.MaxValue).NotNullable()
              .WithColumn("IsPopular").AsBoolean().WithDefaultValue(false);// Common features (JSON)
    }

}
