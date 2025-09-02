using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Reports;
public partial record UsersStatisticsModel : BaseNopModel
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int? Day { get; set; }
    public int Count { get; set; }
}
