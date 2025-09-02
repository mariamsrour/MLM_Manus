using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;

namespace Nop.Services.Catalog;
public interface ICategorySpecGroupService
{
    Task<SpecificationAttributeGroup> GetGroupsByCategoryIdAsync(int categoryId);
    Task InsertMappingAsync(int categoryId, int groupId);
}

