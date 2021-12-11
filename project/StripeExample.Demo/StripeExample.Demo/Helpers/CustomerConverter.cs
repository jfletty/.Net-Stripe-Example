using Stripe;
using StripeExample.Demo.Services.Models;

namespace StripeExample.Demo.Services.Helpers
{
    public static class CustomerConverter
    {
        public static CustomerDTO Convert(Customer stripeCustomer)
        {
            return new()
            {
                ExternalId = stripeCustomer.Id,
                Balance = stripeCustomer.Balance,
                Created = stripeCustomer.Created,
                Currency = stripeCustomer.Currency,
                Deleted = stripeCustomer.Deleted,
                Description = stripeCustomer.Description,
                Email = stripeCustomer.Email,
                Name = stripeCustomer.Name
            };
        }
        
        public static Customer Convert(CustomerDTO customer)
        {
            return new()
            {
                Id = customer.ExternalId,
                Balance = customer.Balance,
                Created = customer.Created,
                Currency = customer.Currency,
                Deleted = customer.Deleted,
                Description = customer.Description,
                Email = customer.Email,
                Name = customer.Name
            };
        }
    }
}