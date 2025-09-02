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
[NopMigration("2025-06-04 00:00:00", "AddCategory-SpecificationAttributeGroupmapping")]
public class AddCategorySpecificationAttributeGroup : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table(nameof(CategorySpecificationAttributeGroup))
            .WithColumn(nameof(CategorySpecificationAttributeGroup.Id)).AsInt32().PrimaryKey().Identity()
            .WithColumn(nameof(CategorySpecificationAttributeGroup.CategoryId)).AsInt32().ForeignKey<Category>()
            .WithColumn(nameof(CategorySpecificationAttributeGroup.SpecificationAttributeGroupId)).AsInt32().ForeignKey<SpecificationAttributeGroup>();
    }
}