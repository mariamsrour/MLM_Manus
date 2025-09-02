using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Vendors;
using Nop.Data;

namespace Nop.Services.Vendors;
public class TagService : ITagService
{

    private readonly IRepository<FeedbackTag> _feedbackTagRepository;
    private readonly IRepository<ReviewTag> _reviewTagRepository;

    public TagService(
        IRepository<ReviewTag> reviewTagRepository,
        IRepository<FeedbackTag> feedbackTagRepository)
    {
        _reviewTagRepository = reviewTagRepository;
        _feedbackTagRepository = feedbackTagRepository;
    }

    public async Task<IList<FeedbackTag>> GetTopTagsForVendorAsync(int vendorId, int count)
    {
        var query = from rt in _reviewTagRepository.Table
                    join ft in _feedbackTagRepository.Table on rt.TagId equals ft.Id
                    where rt.VendorId == vendorId
                    group ft by ft.Tag into g
                    orderby g.Count() descending
                    select new FeedbackTag { Tag = g.Key, Id = g.Count() };

        return await query.Take(count).ToListAsync();
    }

    public async Task<IList<FeedbackTag>> GetTagsForReviewAsync(int reviewId)
    {
        var query = from rt in _reviewTagRepository.Table
                    join ft in _feedbackTagRepository.Table on rt.TagId equals ft.Id
                    where rt.ReviewId == reviewId
                    select ft;

        return await query.ToListAsync();
    }


    public async Task<IList<FeedbackTag>> GetAllFeedbackTagsAsync()
    {
        return await _feedbackTagRepository.Table.Select(x => x).ToListAsync();
    }

    public async Task AddFeedbackTags(int reviewId, int vendor, List<int> tags)
    {
        foreach (var x in tags)
        {
            await _reviewTagRepository.InsertAsync(new ReviewTag { ReviewId = reviewId, VendorId = vendor, TagId = x });

        }
        
    }
}
