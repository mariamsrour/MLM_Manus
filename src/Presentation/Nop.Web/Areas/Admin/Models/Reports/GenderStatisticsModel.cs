using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Reports;
public partial record GenderStatisticsModel : BaseNopModel
{
    public string Gender { get; set; }
    public int Count { get; set; }
}
