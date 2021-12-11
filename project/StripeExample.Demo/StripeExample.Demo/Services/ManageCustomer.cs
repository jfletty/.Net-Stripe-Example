using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        private readonly StripeConfig _stripeConfig;
        private readonly Logger<ManageCustomer> _logger;

        public ManageCustomer(
            IRequestClient requestClient,
            StripeConfig stripeConfig,
            Logger<ManageCustomer> logger)
        {
            _requestClient = requestClient;
            _logger = logger;
            _stripeConfig = stripeConfig;
        }

        public async Task<CustomerDTO> Get(string customerId)
        {
            var parameters = new KeyValuePair<string, string>("", customerId);
            var response = await _requestClient.DoGet<Customer>(_stripeConfig.Url, "customers", parameters);
            return CustomerConverter.Convert(response);
        }

        public async Task<List<CustomerDTO>> GetAll()
        {
            var response = await _requestClient.DoGet<List<Customer>>(_stripeConfig.Url, "customers");
            return response.Select(CustomerConverter.Convert).ToList();
        }

        public async Task<CustomerDTO> CreateOrUpdateCustomer(CustomerDTO customer)
        {
            var response = await _requestClient.DoGet<Customer>(_stripeConfig.Url, "customers");
            return CustomerConverter.Convert(response);
        }

        public async Task<bool> DeleteCustomer(string customerId)
        {
            var parameters = new KeyValuePair<string, string>("", customerId);
            var result = await _requestClient.DoDelete<CustomerDeletedDTO>(_stripeConfig.Url, "customers", parameters);
            return result == null;
        }
    }
}