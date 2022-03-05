using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using Stripe;
using StripeExample.Demo.Services.Clients;
using StripeExample.Demo.Services.Helpers;
using StripeExample.Demo.Services.Models;
using StripeExample.Demo.Services.Services.Interfaces;

namespace StripeExample.Demo.Services.Services
{   
    public class ManageSubscription : IManageSubscription
    {
        public IRequestClient RequestClient;

        public ManageSubscription(StripeConfig config)
        {
            var client = new RestClient(config.Url);
            RequestClient = new RequestClient(client, config.StripeSecret);
        }

        public async Task<SubscriptionDTO> GetAsync(string subscriptionId)
        {
            var parameters = new KeyValuePair<string, string>("", subscriptionId);
            var response = await RequestClient.DoGet<Subscription>("subscriptions", parameters);
            return SubscriptionConverter.Convert(response);
        }

        public async Task<List<SubscriptionDTO>> GetAllAsync()
        {
            var response = await RequestClient.DoGet<List<Subscription>>("subscriptions");
            return response.Select(SubscriptionConverter.Convert).ToList();
        }

        public async Task<SubscriptionDTO> CreateOrUpdateAsync(SubscriptionDTO subscription)
        {
            var body = JsonConvert.SerializeObject(SubscriptionConverter.Convert(subscription));
            var response = await RequestClient.DoPost<Subscription>("subscriptions", body);
            return SubscriptionConverter.Convert(response);
        }
    }
}