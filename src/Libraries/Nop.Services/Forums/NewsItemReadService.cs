using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.News;
using Nop.Data;

namespace Nop.Services.Forums;
public class NewsItemReadService: INewsItemReadService
{
    private readonly IRepository<NewsItem> _newsItemRepository;
    private readonly IRepository<NewsItemRead> _newsItemReadRepository;

    public NewsItemReadService(
        IRepository<NewsItem> newsItemRepository,
        IRepository<NewsItemRead> newsItemReadRepository)
    {
        _newsItemRepository = newsItemRepository;
        _newsItemReadRepository = newsItemReadRepository;
    }

    public async Task<int> GetUnreadCountAsync(int customerId)
    {
        var query = from n in _newsItemRepository.Table
                    where (n.CustomerId == null || n.CustomerId == customerId) && n.Published && (n.EndDateUtc >= DateTime.UtcNow)
                    where !_newsItemReadRepository.Table
                        .Any(r => r.NewsItemId == n.Id && r.CustomerId == customerId)
                    select n;

        return await query.CountAsync();
    }

    public async Task<IList<NewsItem>> GetUnreadAsync(int customerId)
    {
        var query = from n in _newsItemRepository.Table
                    where (n.CustomerId == null || n.CustomerId == customerId) && n.Published && (n.EndDateUtc >= DateTime.UtcNow)
                    where !_newsItemReadRepository.Table
                        .Any(r => r.NewsItemId == n.Id && r.CustomerId == customerId)
                    orderby n.CreatedOnUtc descending
                    select n;

        return await query.ToListAsync();
    }

    public async Task MarkAsReadAsync(int customerId, int newsItemId)
    {
        if (!await _newsItemReadRepository.Table
                .AnyAsync(x => x.NewsItemId == newsItemId && x.CustomerId == customerId))
        {
            await _newsItemReadRepository.InsertAsync(new NewsItemRead
            {
                NewsItemId = newsItemId,
                CustomerId = customerId,
                ReadOnUtc = DateTime.UtcNow
            });
        }
    }

    public async Task MarkAllAsReadAsync(int customerId, IEnumerable<int> newsItemIds)
    {
        var alreadyReadIds = await _newsItemReadRepository.Table
            .Where(x => x.CustomerId == customerId && newsItemIds.Contains(x.NewsItemId))
            .Select(x => x.NewsItemId)
            .ToListAsync();

        var toInsert = newsItemIds
            .Except(alreadyReadIds)
            .Select(id => new NewsItemRead
            {
                NewsItemId = id,
                CustomerId = customerId,
                ReadOnUtc = DateTime.UtcNow
            })
            .ToList();

        if (toInsert.Any())
            await _newsItemReadRepository.InsertAsync(toInsert);
    }
}
