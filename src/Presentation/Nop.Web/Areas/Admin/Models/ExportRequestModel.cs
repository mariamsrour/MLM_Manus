using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.ExportToExcelPlugin;
public class ExportRequestModel
{
    public string ViewName { get; set; }
    public IEnumerable<Dictionary<string, object>> Data { get; set; }
    public IEnumerable<string> Columns { get; set; }
}