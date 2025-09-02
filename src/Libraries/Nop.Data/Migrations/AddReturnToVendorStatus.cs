using System.Data;
using FluentMigrator;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Data.Extensions;

namespace Nop.Data.Migrations;
[NopSchemaMigration("2024-11-30 00:00:00", "ReturnRequest.AddReturnToVendorStatus")]

public partial class AddReturnToVendorStatus : ForwardOnlyMigration
{
    public override void Up()
    {
        var customerTableName = nameof(ReturnRequest);

        // Check if the WalletBalance column already exists
        if (!Schema.Table(customerTableName).Column(nameof(ReturnRequest.ReturnedToVendorStatusId)).Exists())
        {
            Alter.Table(customerTableName)
                .AddColumn(nameof(ReturnRequest.ReturnedToVendorStatusId)).AsInt32().NotNullable().WithDefaultValue(0);
        }
    }
}
