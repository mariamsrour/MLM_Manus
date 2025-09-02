using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Catalog;
public partial class CategorySpecificationAttributeGroup : BaseEntity
{
    public int CategoryId { get; set; }
    public int SpecificationAttributeGroupId { get; set; }

    public virtual Category Category { get; set; }
    public virtual SpecificationAttributeGroup SpecificationAttributeGroup { get; set; }
}