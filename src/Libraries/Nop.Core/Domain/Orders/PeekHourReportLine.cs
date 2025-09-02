using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Orders;
public partial class PeekHourReportLine
{

    public int CountryId { get; set; }
    public string CountryName { get; set; }
    public List<int> PeakHour { get; set; } // The hour of the day (0-23)
    public List<int> NumberOfOrders { get; set; }
    public int Rank { get; set; }
}
