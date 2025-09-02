using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Common;
public class AdReports :BaseEntity
{
    public int ReportReason { get; set; }
    public int AdId { get; set; }
    public string Description { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public int CustomerId { get; set; }

}

public class AdReportReason : BaseEntity
{
    public int LanguageId { get; set; }
    public string Title { get; set; }

}
