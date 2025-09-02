using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;
using Microsoft.AspNetCore.Http.HttpResults;
using Nop.Core.Domain.Catalog;

namespace Nop.Data.Migrations;
[NopMigration("2025-05-14 01:00:00", "Catalog.TempProductPicturebaseschema")]
public class TempProductPictureMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table(nameof(TempProductPicture))
            .WithColumn(nameof(TempProductPicture.Id)).AsInt32().PrimaryKey().Identity()
            .WithColumn(nameof(TempProductPicture.PictureId)).AsInt32().NotNullable()
            .WithColumn(nameof(TempProductPicture.SessionId)).AsString(128).NotNullable()
            .WithColumn(nameof(TempProductPicture.CreatedOnUtc)).AsDateTime().NotNullable()
            .WithColumn(nameof(TempProductPicture.DisplayOrder)).AsInt32().NotNullable().WithDefaultValue(0);
    }
}

