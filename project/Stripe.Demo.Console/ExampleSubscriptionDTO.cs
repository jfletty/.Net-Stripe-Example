using StripeExample.Demo.Services.Models;

namespace Stripe.Demo.Console;

public static class ExampleSubscriptionDTO
{
    public static SubscriptionDTO Build(string customerId)
    {
        return new SubscriptionDTO
        {
            Customer = customerId,
            CollectionMethod = CollectionMethod.charge_automatically,
            Status = SubscriptionStatus.active
        };
    }
}