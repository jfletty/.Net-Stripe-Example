using System;
using Stripe;
using StripeExample.Demo.Services.Models;

namespace StripeExample.Demo.Services.Helpers
{
    public static class SubscriptionConverter
    {
        public static SubscriptionDTO Convert(Subscription stripeSubscription)
        {
            return new()
            {
                Id = stripeSubscription.Id,
                Customer = stripeSubscription.Customer.Id,
                Status = Enum.Parse<SubscriptionStatus>(stripeSubscription.Status),
                CollectionMethod = Enum.Parse<CollectionMethod>(stripeSubscription.CollectionMethod)
            };
        }
        
        public static Subscription Convert(SubscriptionDTO subscription)
        {
            return new()
            {
                Id = subscription.Id,
                Status = subscription.Status.ToString(),
                CollectionMethod = subscription.CollectionMethod.ToString()
            };
        }
    }
}