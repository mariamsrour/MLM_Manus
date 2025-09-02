using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nop.Core;
using Nop.Core.Domain.Subscriptions;
using Nop.Data;

namespace Nop.Services.Subscription;
public class SubscriptionService : ISubscriptionService
{
    private readonly IRepository<Plan> _planRepository;
    private readonly IRepository<Package> _packageRepository;
    private readonly IRepository<UserSubscription> _userSubscriptionRepository;
    private readonly IWorkContext _workContext;

    public SubscriptionService(
        IRepository<Plan> planRepository,
        IRepository<Package> packageRepository,
        IRepository<UserSubscription> userSubscriptionRepository,
        IWorkContext workContext)
    {
        _planRepository = planRepository;
        _packageRepository = packageRepository;
        _userSubscriptionRepository = userSubscriptionRepository;
        _workContext = workContext;
    }

    public async Task<List<Plan>> GetAvailablePlansWithPackagesAsync()
    {
        var plans  = await _planRepository.Table
            .Include(p => p.Packages)
            .ToListAsync();
        foreach (var plan in plans)
        {
            plan.Packages = await _packageRepository.Table.Where(p => p.PlanId == plan.Id).ToListAsync();
        }
        return plans;
    }

    public async Task<UserSubscription> CreateSubscriptionAsync(int customerId, int packageId, double price, int catId)
    {
        var package = await _packageRepository.GetByIdAsync(packageId)
            ?? throw new NopException("Package not found");

        //var existingSubscription = await GetActiveSubscriptionAsync(customerId);
        //if (existingSubscription != null)
        //{
        //    // Handle subscription upgrade/downgrade logic
        //    existingSubscription.IsActive = false;
        //    await _userSubscriptionRepository.UpdateAsync(existingSubscription);
        //}
        var pp = decimal.Parse(price.ToString());
        var subscription = new UserSubscription
        {
            CustomerId = customerId,
            PackageId = packageId,
            StartDate = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow.AddDays(package.DurationDays),
            HighlightedUsed = 0,
            NormalUsed = 0,
            IsActive = true,
            PaidPrice = pp,
            CategoryId = catId
        };

        var id  = await _userSubscriptionRepository.InsertIdAsync(subscription);
        subscription.Id = id;
        return subscription;
    }

    public async Task<List<UserSubscription>> GetActiveSubscriptionAsync(int customerId, int catId)
    {
        var res = await _userSubscriptionRepository.Table

            .OrderByDescending(us => us.ExpirationDate)
            .Where(us =>
                us.CustomerId == customerId &&
                us.IsActive &&
                us.ExpirationDate > DateTime.UtcNow &&
                us.CategoryId == catId)
            .ToListAsync();
        foreach (var p in res)
        {
            p.Package = await _packageRepository.GetByIdAsync(p.PackageId);
            p.Package.Plan = await _planRepository.GetByIdAsync(p.Package.PlanId);
        }

        return res;
    }

    public async Task<List<UserSubscription>> GetAllSubscriptionAsync(int customerId)
    {
        var res = await _userSubscriptionRepository.Table

            .OrderByDescending(us => us.ExpirationDate)
            .Where(us =>
                us.CustomerId == customerId )
            .ToListAsync();
        foreach (var p in res)
        {
            p.Package = await _packageRepository.GetByIdAsync(p.PackageId);
            p.Package.Plan = await _planRepository.GetByIdAsync(p.Package.PlanId);
        }

        return res;
    }

    public async Task IncrementAdUsageAsync(int subscriptionId)
    {
        var subscription = await _userSubscriptionRepository.GetByIdAsync(subscriptionId)
            ?? throw new NopException("Subscription not found");

        var package = await _packageRepository.GetByIdAsync(subscription.PackageId)
            ?? throw new NopException("Package not found");
        bool isHighlighted = package.HighlightedAds > 0;

        if (isHighlighted)
        {
            if (subscription.HighlightedUsed >= package.HighlightedAds)
                throw new NopException("Highlighted ads quota exceeded");

            subscription.HighlightedUsed++;
        }
        else
        {
            if (subscription.NormalUsed >= package.NormalAds)
                throw new NopException("Normal ads quota exceeded");

            subscription.NormalUsed++;
        }

        // Automatically deactivate if ads are exhausted
        //if (subscription.HighlightedUsed + subscription.NormalUsed >=
        //    package.HighlightedAds + package.NormalAds)
        //{
        //    subscription.IsActive = false;
        //}

        await _userSubscriptionRepository.UpdateAsync(subscription);
    }

    public async Task<Plan> GetUserSubscriptionByIdAsync(int pack)
    {
        var package = await _packageRepository.GetByIdAsync(pack);
        var plan = await _planRepository.GetByIdAsync(package.PlanId);
        plan.Packages = new List<Package>();
        plan.Packages.Add(package);
        return plan;
    }
}
