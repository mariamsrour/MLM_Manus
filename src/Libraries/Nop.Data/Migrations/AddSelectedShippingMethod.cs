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
using Nop.Core.Domain.Orders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Nop.Data.Migrations;
[NopSchemaMigration("2025-02-01 00:00:00", "ShoppingCartItem.SelectedShippingMethod")]
public partial class AddSelectedShippingMethod: ForwardOnlyMigration
{
    public override void Up()
    {
        var TableName = nameof(ShoppingCartItem);


        if (!Schema.Table(TableName).Column(nameof(ShoppingCartItem.SelectedShippingMethod)).Exists())
        {
            Alter.Table(TableName)
                .AddColumn(nameof(ShoppingCartItem.SelectedShippingMethod)).AsInt32().NotNullable().WithDefaultValue(0);
        }
    }

}
