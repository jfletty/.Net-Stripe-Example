using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StripeExample.Demo.Services.Clients;
using StripeExample.Demo.Services.Helpers;
using StripeExample.Demo.Services.Models;
using StripeExample.Demo.Services.Services.Interfaces;
using Customer = Stripe.Customer;

namespace StripeExample.Demo.Services.Services
{
    public class ManageCustomer : IManageCustomer
    {
        private readonly IRequestClient _requestClient;
        
        public ManageCustomer(StripeConfig config)
        {
            _requestClient = new RequestClient(config.Url, config.StripeSecret);
        }

        public async Task<CustomerDTO> GetAsync(string customerId)
        {
            var parameters = new KeyValuePair<string, string>("", customerId);
            var response = await _requestClient.DoGet<Customer>("customers", parameters);
            return CustomerConverter.Convert(response);
        }

        public async Task<List<CustomerDTO>> GetAllAsync()
        {
            var response = await _requestClient.DoGet<List<Customer>>("customers");
            return response.Select(CustomerConverter.Convert).ToList();
        }

        public async Task<CustomerDTO> CreateOrUpdateCustomerAsync(CustomerDTO customer)
        {
            var body = JsonConvert.SerializeObject(CustomerConverter.Convert(customer));
            var response = await _requestClient.DoPost<Customer>("customers", body);
            return CustomerConverter.Convert(response);
        }

        public async Task<bool> DeleteCustomerAsync(string customerId)
        {
            var parameters = new KeyValuePair<string, string>("", customerId);
            var result = await _requestClient.DoDelete<CustomerDeletedDTO>("customers", parameters);
            return result == null;
        }
    }
}