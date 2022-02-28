using System.Collections.Generic;
using System.Threading.Tasks;
using Stripe;
using StripeExample.Demo.Services.Clients;
using StripeExample.Demo.Services.Helpers;
using StripeExample.Demo.Services.Models;
using StripeExample.Demo.Services.Services.Interfaces;

namespace StripeExample.Demo.Services.Services
{   
    public class ManageSubscription : IManageSubscription
    {
        private readonly IRequestClient _requestClient;

        public ManageSubscription(StripeConfig config)
        {
            _requestClient = new RequestClient(config.Url, config.StripeSecret);
        }

        public async Task<SubscriptionDTO> GetAsync(string subscriptionId)
        {
            var parameters = new KeyValuePair<string, string>("", subscriptionId);
            var response = await _requestClient.DoGet<Subscription>("subscriptions", parameters);
            return SubscriptionConverter.Convert(response);
        }

        public async Task<List<SubscriptionDTO>> GetAllAsync(string customerId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<SubscriptionDTO> CreateOrUpdateAsync(SubscriptionDTO subscription)
        {
            throw new System.NotImplementedException();
        }
    }
}