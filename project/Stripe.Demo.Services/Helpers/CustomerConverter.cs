using Stripe;
using StripeExample.Demo.Services.Models;

namespace StripeExample.Demo.Services.Helpers
{
    public static class CustomerConverter
    {
        public static CustomerDTO Convert(Customer stripeCustomer)
        {
            if (stripeCustomer == null) return null;
            return new()
            {
                ExternalId = stripeCustomer.Id,
                Balance = stripeCustomer.Balance,
                Currency = stripeCustomer.Currency,
                Deleted = stripeCustomer.Deleted,
                Description = stripeCustomer.Description,
                Email = stripeCustomer.Email,
                Name = stripeCustomer.Name,
                Phone = stripeCustomer.Phone
            };
        }
        
        public static Customer Convert(CustomerDTO customer)
        {
            if (customer == null) return null;
            return new()
            {
                Id = customer.ExternalId,
                Balance = customer.Balance,
                Currency = customer.Currency,
                Deleted = customer.Deleted,
                Description = customer.Description,
                Email = customer.Email,
                Name = customer.Name,
                Phone = customer.Phone
            };
        }
    }
}