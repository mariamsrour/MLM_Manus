using System.Data;
using FluentMigrator;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Data.Extensions;

namespace Nop.Data.Migrations;

[NopSchemaMigration("2024-11-25 00:00:00", "Customer.AddWalletBalance")]

public partial class AddWalletTransaction : ForwardOnlyMigration
{
    public override void Up()
    {
        //migrationBuilder.CreateTable(
        //    name: "WalletTransaction",
        //    columns: table => new
        //    {
        //        Id = table.Column<int>(nullable: false)
        //            .Annotation("SqlServer:Identity", "1, 1"),
        //        CustomerId = table.Column<int>(nullable: false),
        //        Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
        //        TransactionType = table.Column<string>(maxLength: 50, nullable: false),
        //        Description = table.Column<string>(maxLength: 500, nullable: true),
        //        CurrencyCode = table.Column<string>(maxLength: 10, nullable: false),
        //        CreatedOnUtc = table.Column<DateTime>(nullable: false)
        //    },
        //    constraints: table =>
        //    {
        //        table.PrimaryKey("PK_WalletTransaction", x => x.Id);
        //        table.ForeignKey(
        //            name: "FK_WalletTransaction_Customer_CustomerId",
        //            column: x => x.CustomerId,
        //            principalTable: "Customer",
        //            principalColumn: "Id",
        //            onDelete: ReferentialAction.Cascade);
        //    });

        //migrationBuilder.AddColumn<decimal>(
        //    name: "WalletBalance",
        //    table: "Customer",
        //    type: "decimal(18, 2)",
        //    nullable: false,
        //    defaultValue: 0);

        //migrationBuilder.AddColumn<string>(
        //    name: "BankTransactionNumber",
        //    table: "Order",
        //    type: "string",
        //    nullable: true);

        //migrationBuilder.AddColumn<string>(
        //    name: "ZactaURL",
        //    table: "Order",
        //    type: "string",
        //    nullable: true);

        //migrationBuilder.AddColumn<string>(
        //    name: "ZactaID",
        //    table: "Order",
        //    type: "string",
        //    nullable: true);
        Create.TableFor<WalletTransaction>();

        var customerTableName = nameof(Customer);

        // Check if the WalletBalance column already exists
        if (!Schema.Table(customerTableName).Column(nameof(Customer.WalletBalance)).Exists())
        {
            Alter.Table(customerTableName)
                .AddColumn(nameof(Customer.WalletBalance)).AsDecimal(18, 2).NotNullable().WithDefaultValue(0);
        }
        var orderTableName = nameof(Order);

        // Add BankTransactionNumber column if it doesn't exist
        if (!Schema.Table(orderTableName).Column(nameof(Order.BankTransactionNumber)).Exists())
        {
            Alter.Table(orderTableName)
                .AddColumn(nameof(Order.BankTransactionNumber)).AsString(int.MaxValue).Nullable();
        }

        // Add ZactaID column if it doesn't exist
        if (!Schema.Table(orderTableName).Column(nameof(Order.ZactaID)).Exists())
        {
            Alter.Table(orderTableName)
                .AddColumn(nameof(Order.ZactaID)).AsString(int.MaxValue).Nullable();
        }

        // Add ZactaURL column if it doesn't exist
        if (!Schema.Table(orderTableName).Column(nameof(Order.ZactaURL)).Exists())
        {
            Alter.Table(orderTableName)
                .AddColumn(nameof(Order.ZactaURL)).AsString(int.MaxValue).Nullable();
        }

    }


}

