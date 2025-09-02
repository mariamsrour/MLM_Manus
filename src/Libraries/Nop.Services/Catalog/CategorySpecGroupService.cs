using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Data;

namespace Nop.Services.Catalog;
public class CategorySpecGroupService : ICategorySpecGroupService
{
    private readonly IRepository<CategorySpecificationAttributeGroup> _mappingRepo;
    private readonly IRepository<SpecificationAttributeGroup> _groupRepo;

    public CategorySpecGroupService(IRepository<CategorySpecificationAttributeGroup> mappingRepo,
        IRepository<SpecificationAttributeGroup> groupRepo)
    {
        _mappingRepo = mappingRepo;
        _groupRepo = groupRepo;
    }

    public async Task<SpecificationAttributeGroup> GetGroupsByCategoryIdAsync(int categoryId)
    {
        var groupIds = await _mappingRepo.Table
            .Where(m => m.CategoryId == categoryId)
            .Select(m => m.SpecificationAttributeGroupId)
            .FirstOrDefaultAsync();

        return await _groupRepo.Table.FirstOrDefaultAsync(g => g.Id == groupIds);
    }

    public async Task InsertMappingAsync(int categoryId, int groupId)
    {
        if (categoryId <= 0 || groupId <= 0)
            throw new ArgumentException("Invalid categoryId or groupId");

        var mapping = new CategorySpecificationAttributeGroup
        {
            CategoryId = categoryId,
            SpecificationAttributeGroupId = groupId
        };

        await _mappingRepo.InsertAsync(mapping);
    }

}
