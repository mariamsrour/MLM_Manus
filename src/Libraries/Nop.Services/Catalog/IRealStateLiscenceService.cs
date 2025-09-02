using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Data;

namespace Nop.Services.Catalog;
public partial interface IRealStateLiscenceService
{
    Task AddLiscence(RealStateLiscence liscence);
}
