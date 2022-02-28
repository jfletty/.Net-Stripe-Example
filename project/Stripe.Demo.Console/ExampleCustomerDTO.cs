using StripeExample.Demo.Services.Models;

namespace Stripe.Demo.Console;

public static class ExampleCustomerDTO
{
    public static CustomerDTO Build()
    {
        return new CustomerDTO
        {
            Name = "John Doe",
            Email = "John.Doe@example.com",
            Currency = "USD",
            Description = "Example Customer",
            Phone = "0800 83 83 83"
        };
    }
}