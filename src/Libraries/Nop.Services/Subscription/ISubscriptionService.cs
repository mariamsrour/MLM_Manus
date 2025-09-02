using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Subscriptions;

namespace Nop.Services.Subscription;
public interface ISubscriptionService
{
    Task<List<Plan>> GetAvailablePlansWithPackagesAsync();
    Task<UserSubscription> CreateSubscriptionAsync(int customerId, int packageId, double price, int catId);
    Task<List<UserSubscription>> GetActiveSubscriptionAsync(int customerId, int catId);
    Task<List<UserSubscription>> GetAllSubscriptionAsync(int customerId);
    Task IncrementAdUsageAsync(int subscriptionId);
    Task<Plan> GetUserSubscriptionByIdAsync(int pack);
}