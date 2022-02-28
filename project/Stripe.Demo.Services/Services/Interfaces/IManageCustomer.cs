using System.Collections.Generic;
using System.Threading.Tasks;
using StripeExample.Demo.Services.Models;

namespace StripeExample.Demo.Services.Services.Interfaces
{
    public interface IManageCustomer
    {
        Task<CustomerDTO> GetAsync(string customerId);
        Task<List<CustomerDTO>> GetAllAsync();
        Task<CustomerDTO> CreateOrUpdateCustomerAsync(CustomerDTO customer);
        Task<bool> DeleteCustomerAsync(string customerId);
    }
}