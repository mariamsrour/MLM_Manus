using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.News;

namespace Nop.Services.Forums;
public interface INewsItemReadService
{
    Task<int> GetUnreadCountAsync(int customerId);
    Task<IList<NewsItem>> GetUnreadAsync(int customerId);
    Task MarkAsReadAsync(int customerId, int newsItemId);
    Task MarkAllAsReadAsync(int customerId, IEnumerable<int> newsItemIds);
}
