using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace Nop.Data.Migrations;
[NopSchemaMigration("2025-03-03 01:00:00", "Add icon to specification attribute options")]
public class AddSpecificationAttributeOptionIcon : ForwardOnlyMigration
{
    public override void Up()
    {
        if (!Schema.Table("SpecificationAttributeOption").Column("Icon").Exists())
        {
            Alter.Table("SpecificationAttributeOption")
                .AddColumn("Icon").AsString(500).Nullable();
        }
    }
}
