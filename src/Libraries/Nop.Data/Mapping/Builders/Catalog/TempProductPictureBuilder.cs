using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Catalog;

namespace Nop.Data.Mapping.Builders.Catalog;
public class TempProductPictureBuilder : NopEntityBuilder<TempProductPicture>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table.WithColumn(nameof(TempProductPicture.PictureId)).AsInt32().NotNullable();
        table.WithColumn(nameof(TempProductPicture.SessionId)).AsString(128).NotNullable();
        table.WithColumn(nameof(TempProductPicture.CreatedOnUtc)).AsDateTime().NotNullable();
        table.WithColumn(nameof(TempProductPicture.DisplayOrder)).AsInt32().NotNullable().WithDefaultValue(0);
    }
}
