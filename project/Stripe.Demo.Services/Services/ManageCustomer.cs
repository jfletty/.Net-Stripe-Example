using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using StripeExample.Demo.Services.Clients;
using StripeExample.Demo.Services.Helpers;
using StripeExample.Demo.Services.Models;
using StripeExample.Demo.Services.Services.Interfaces;
using Customer = Stripe.Customer;

namespace StripeExample.Demo.Services.Services
{
    public class ManageCustomer : IManageCustomer
    {
        public IRequestClient RequestClient;
        
        public ManageCustomer(StripeConfig config)
        {
            var client = new RestClient(config.Url);
            RequestClient = new RequestClient(client, config.StripeSecret);
        }

        public async Task<CustomerDTO> GetAsync(string customerId)
        {
            var parameters = new KeyValuePair<string, string>("", customerId);
            var response = await RequestClient.DoGet<Customer>("customers", parameters);
            return CustomerConverter.Convert(response);
        }

        public async Task<List<CustomerDTO>> GetAllAsync()
        {
            var response = await RequestClient.DoGet<List<Customer>>("customers");
            return response.Select(CustomerConverter.Convert).ToList();
        }

        public async Task<CustomerDTO> CreateOrUpdateCustomerAsync(CustomerDTO customer)
        {
            var body = JsonConvert.SerializeObject(CustomerConverter.Convert(customer));
            var response = await RequestClient.DoPost<Customer>("customers", body);
            return CustomerConverter.Convert(response);
        }

        public async Task<bool> DeleteCustomerAsync(string customerId)
        {
            var parameters = new KeyValuePair<string, string>("", customerId);
            var result = await RequestClient.DoDelete<CustomerDeletedDTO>("customers", parameters);
            return result == null;
        }
    }
}