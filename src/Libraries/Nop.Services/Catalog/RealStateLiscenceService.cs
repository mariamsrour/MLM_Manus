using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Data;

namespace Nop.Services.Catalog;
public class RealStateLiscenceService : IRealStateLiscenceService
{
    private readonly IRepository<RealStateLiscence> _groupRepo;

    public RealStateLiscenceService(IRepository<RealStateLiscence> groupRepo)
    {
        _groupRepo = groupRepo;
    }

    public async Task AddLiscence(RealStateLiscence liscence)
    {
        await _groupRepo.InsertAsync(liscence);
    }
}
