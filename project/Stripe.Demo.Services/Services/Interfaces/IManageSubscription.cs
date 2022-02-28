using System.Collections.Generic;
using System.Threading.Tasks;
using StripeExample.Demo.Services.Models;

namespace StripeExample.Demo.Services.Services.Interfaces
{
    public interface IManageSubscription
    {
        Task<SubscriptionDTO> GetAsync(string subscriptionId);
        Task<List<SubscriptionDTO>> GetAllAsync(string customerId);
        Task<SubscriptionDTO> CreateOrUpdateAsync(SubscriptionDTO subscription);
    }
}