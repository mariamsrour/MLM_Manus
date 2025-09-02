using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;
using Microsoft.AspNetCore.Http.HttpResults;
using Nop.Core.Domain.Catalog;
using Nop.Data.Extensions;

namespace Nop.Data.Migrations;
[NopMigration("2025-06-04 01:00:00", "AddRealstateLiscence")]
public class AddRealstateLiscence : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table(nameof(RealStateLiscence))
            .WithColumn(nameof(RealStateLiscence.Id)).AsInt32().PrimaryKey().Identity()
            .WithColumn(nameof(RealStateLiscence.HolderName)).AsString(255).NotNullable()
            .WithColumn(nameof(RealStateLiscence.HolderId)).AsString(255).NotNullable()
            .WithColumn(nameof(RealStateLiscence.LiscenceNumber)).AsString(255).NotNullable()
            .WithColumn(nameof(RealStateLiscence.CreatedOnUtc)).AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn(nameof(RealStateLiscence.ProductId)).AsInt32().ForeignKey<Product>();
    }
}