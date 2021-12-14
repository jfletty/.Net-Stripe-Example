using System.Collections.Generic;
using System.Threading.Tasks;
using StripeExample.Demo.Services.Models;

namespace StripeExample.Demo.Services.Services.Interfaces
{
    public interface IManageCustomer
    {
        Task<CustomerDTO> Get(string customerId);
        Task<List<CustomerDTO>> GetAll();
        Task<CustomerDTO> CreateOrUpdateCustomer(CustomerDTO customer);
        Task<bool> DeleteCustomer(string customerId);
    }
}