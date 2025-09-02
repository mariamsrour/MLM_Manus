using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Orders;
public class CategoryOrderReportLine
{
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int NumberOfOrders { get; set; }
        public int Rank { get; set; }
    
}

public class VendorOrderReportLine
{

        public int Rank { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public int NumberOfOrders { get; set; }
    
}
