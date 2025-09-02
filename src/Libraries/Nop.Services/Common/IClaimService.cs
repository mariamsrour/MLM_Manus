using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Common;

namespace Nop.Services.Common;
public interface IClaimService
{
    Task InsertClaimAsync(CustomerClaims claim);
    Task UpdateClaimAsync(CustomerClaims claim);
    Task<IPagedList<CustomerClaims>> GetAllClaimsAsync(string search = "", ClaimStatus? status = null, DateTime? startDate = null, DateTime? endDate = null, int page = 1, int pagesize = 10);
    Task<CustomerClaims> GetClaimByIdAsync(int id);
}
