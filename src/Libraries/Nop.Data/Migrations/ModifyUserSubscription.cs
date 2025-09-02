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

namespace Nop.Data.Migrations;
[NopSchemaMigration("2025-04-25 01:00:00", "ModifyUserSubscription")]
public partial class ModifyUserSubscription : ForwardOnlyMigration
{
    public override void Up()
    {
        var userSubscription = nameof(UserSubscription);

        if (!Schema.Table(userSubscription).Column(nameof(UserSubscription.PaidPrice)).Exists())
        {
            Alter.Table(userSubscription)
                .AddColumn(nameof(UserSubscription.PaidPrice)).AsDecimal().NotNullable().WithDefaultValue(0);
        }
    }

}
