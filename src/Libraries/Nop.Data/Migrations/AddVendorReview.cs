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
[NopSchemaMigration("2025-04-03 00:00:00", "VendorReview")]
public partial class AddVendorReview : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("VendorReview")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("VendorId").AsInt32().NotNullable().ForeignKey("FK_VendorReview_Vendor", "Vendor", "Id")
            .WithColumn("CustomerId").AsInt32().NotNullable()
            .WithColumn("IsApproved").AsBoolean().NotNullable()
            .WithColumn("Title").AsString(400).NotNullable()
            .WithColumn("ReviewText").AsString().NotNullable()
            .WithColumn("Rating").AsInt32().NotNullable()
            .WithColumn("CreatedOnUtc").AsDateTime().NotNullable();
    }

}
