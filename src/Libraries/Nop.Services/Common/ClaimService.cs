using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Data;

namespace Nop.Services.Common;
public class ClaimService : IClaimService
{
    private readonly IRepository<CustomerClaims> _claimRepository;
    public ClaimService(IRepository<CustomerClaims> claimRepository)
    {
        _claimRepository = claimRepository;
    }

    public async Task InsertClaimAsync(CustomerClaims claim) => await _claimRepository.InsertAsync(claim);
    public async Task UpdateClaimAsync(CustomerClaims claim) => await _claimRepository.UpdateAsync(claim);
    public async Task<IPagedList<CustomerClaims>> GetAllClaimsAsync(string search = "", ClaimStatus? status = null, DateTime? startDate = null, DateTime? endDate = null, int page = 1, int pagesize = 10)
    {
        var query = _claimRepository.Table;

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(c => c.Title.Contains(search));

        if (status.HasValue)
            query = query.Where(c => c.Status == (int) status.Value);
        if (startDate != null)
            query = query.Where(c => c.CreatedOnUtc >= startDate.Value);
        if (endDate != null)
            query = query.Where(c => c.CreatedOnUtc <= endDate.Value);

        return await query.OrderByDescending(c => c.CreatedOnUtc).ToPagedListAsync(page,pagesize);
    }
    public async Task<CustomerClaims> GetClaimByIdAsync(int id) => await _claimRepository.GetByIdAsync(id);

}
