using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Vendors;

namespace Nop.Services.Vendors;
public interface ITagService
{
    Task<IList<FeedbackTag>> GetTopTagsForVendorAsync(int vendorId, int count);
    Task<IList<FeedbackTag>> GetTagsForReviewAsync(int reviewId);

    Task<IList<FeedbackTag>> GetAllFeedbackTagsAsync();

    Task AddFeedbackTags(int reviewId, int vendor, List<int> tags);



}
