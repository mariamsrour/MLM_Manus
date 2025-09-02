using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Common;

namespace Nop.Services.Common;
public interface IReportService
{
    Task InsertReportAsync(AdReports report);
    Task UpdateReportAsync(AdReports report);
    Task<IPagedList<AdReports>> GetAllReportsAsync(int reportReason, DateTime? startDate = null, DateTime? endDate = null, int pageIndex = 0, int pageSize = int.MaxValue);
    Task<AdReports> GetReportByIdAsync(int id);
    Task InsertReportResonAsync(AdReportReason report);
    Task<List<AdReportReason>> GetAllReportReasonsAsync();
    Task<AdReportReason> GetResonById(int id);

    Task DeleteReportResonAsync(AdReportReason report);
}
