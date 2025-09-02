namespace Nop.Services.Vendors;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Vendors;
using Nop.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class VendorReviewService : IVendorReviewService
{
    private readonly IRepository<VendorReview> _vendorReviewRepository;
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<OrderItem> _orderItemRepository;
    private readonly IRepository<Product> _productRepository;

    

    public VendorReviewService(
        IRepository<VendorReview> vendorReviewRepository,
        IRepository<Order> orderRepository,
        IRepository<OrderItem> orderItemRepository,
        IRepository<Product> productRepository)
    {
        _vendorReviewRepository = vendorReviewRepository;
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
        _productRepository = productRepository;
    }

    public async Task<int> InsertVendorReviewAsync(VendorReview vendorReview)
    {
       return await _vendorReviewRepository.InsertIdAsync(vendorReview);
    }

    public async Task UpdateVendorReviewAsync(VendorReview vendorReview)
    {
        await _vendorReviewRepository.UpdateAsync(vendorReview);
    }

    public async Task DeleteVendorReviewAsync(VendorReview vendorReview)
    {
        await _vendorReviewRepository.DeleteAsync(vendorReview);
    }

    public async Task<VendorReview> GetVendorReviewByIdAsync(int vendorReviewId)
    {
        return await _vendorReviewRepository.GetByIdAsync(vendorReviewId);
    }

    public async Task<IList<VendorReview>> GetVendorReviewsByVendorIdAsync(int vendorId, bool approvedOnly = false)
    {
        var query = _vendorReviewRepository.Table
            .Where(vr => vr.VendorId == vendorId);

        if (approvedOnly)
            query = query.Where(vr => vr.IsApproved);

        return await query.ToListAsync();
    }

    public async Task<bool> CanCustomerReviewVendorAsync(int customerId, int vendorId)
    {
        // Check if customer already submitted a review for this vendor
        var existingReview = await _vendorReviewRepository.Table
            .AnyAsync(vr => vr.CustomerId == customerId && vr.VendorId == vendorId);

        if (existingReview)
            return false;

        // Check if customer has purchased from this vendor
        var query = from o in _orderRepository.Table
                    join oi in _orderItemRepository.Table on o.Id equals oi.OrderId
                    join p in _productRepository.Table on oi.ProductId equals p.Id
                    where o.CustomerId == customerId &&
                          o.OrderStatusId == (int)OrderStatus.Complete &&
                          p.VendorId == vendorId
                    select o.Id;

        return await query.AnyAsync();
    }
}
