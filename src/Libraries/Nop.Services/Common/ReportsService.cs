using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB;
using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Data;

namespace Nop.Services.Common;
public class ReportsService : IReportService
{
    private readonly IRepository<AdReports> _reportRepository;

    private readonly IRepository<AdReportReason> _reportReasonRepository;
    public ReportsService(IRepository<AdReports> reportRepository,
        IRepository<AdReportReason> reportReasonRepository)
    {
        _reportRepository = reportRepository;
        _reportReasonRepository = reportReasonRepository;
    }

    public async Task InsertReportAsync(AdReports report) => await _reportRepository.InsertAsync(report);
    public async Task UpdateReportAsync(AdReports report) => await _reportRepository.UpdateAsync(report);
    public async Task<IPagedList<AdReports>> GetAllReportsAsync( int reportReason, DateTime? startDate = null, DateTime? endDate = null, int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var query = _reportRepository.Table;

   

        return await query.OrderByDescending(c => c.CreatedOnUtc).ToPagedListAsync(pageIndex, pageSize);
    }
    public async Task<AdReports> GetReportByIdAsync(int id) => await _reportRepository.GetByIdAsync(id);

    public async Task InsertReportResonAsync(AdReportReason report) => await _reportReasonRepository.InsertAsync(report);


    public async Task<List<AdReportReason>> GetAllReportReasonsAsync()
    {
        var query = _reportReasonRepository.Table;

        return await query.ToListAsync();
    }

    public async Task<AdReportReason> GetResonById(int id)
    {
        return await _reportReasonRepository.GetByIdAsync(id);

    }


    public async Task DeleteReportResonAsync(AdReportReason report) => await _reportReasonRepository.DeleteAsync(report);


}
