using System.Data;
using FluentMigrator.Builders.Create.Table;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Customers;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;

namespace Nop.Data.Mapping.Customers
{
    public partial class WalletTransactionMap : NopEntityBuilder<WalletTransaction>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(WalletTransaction.CustomerId)).AsInt32().ForeignKey<Customer>(onDelete: Rule.Cascade).NotNullable()
                .WithColumn(nameof(WalletTransaction.Amount)).AsDecimal(18, 2).NotNullable()
                .WithColumn(nameof(WalletTransaction.TransactionType)).AsString(50).NotNullable()
                .WithColumn(nameof(WalletTransaction.Description)).AsString(500).Nullable()
                .WithColumn(nameof(WalletTransaction.CurrencyCode)).AsString(10).NotNullable()
                .WithColumn(nameof(WalletTransaction.CreatedOnUtc)).AsDateTime2().NotNullable();
        }
    }
}
