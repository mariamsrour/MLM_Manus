using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.Vendors;
public interface IVendorReviewService
{
    Task<int>InsertVendorReviewAsync(VendorReview vendorReview);
    Task UpdateVendorReviewAsync(VendorReview vendorReview);
    Task DeleteVendorReviewAsync(VendorReview vendorReview);
    Task<VendorReview> GetVendorReviewByIdAsync(int vendorReviewId);
    Task<IList<VendorReview>> GetVendorReviewsByVendorIdAsync(int vendorId, bool approvedOnly = false);
    Task<bool> CanCustomerReviewVendorAsync(int customerId, int vendorId);
}
