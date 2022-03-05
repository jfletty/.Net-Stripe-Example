using System;
using Stripe;
using StripeExample.Demo.Services.Models;

namespace StripeExample.Demo.Services.Helpers
{
    public static class SubscriptionConverter
    {
        public static SubscriptionDTO Convert(Subscription stripeSubscription)
        {
            if (stripeSubscription == null) return null;
            return new()
            {
                ExternalId = stripeSubscription.Id,
                Status = Enum.Parse<SubscriptionStatus>(stripeSubscription.Status),
                CollectionMethod = Enum.Parse<CollectionMethod>(stripeSubscription.CollectionMethod)
            };
        }
        
        public static Subscription Convert(SubscriptionDTO subscription)
        {
            if (subscription == null) return null;
            return new()
            {
                Id = subscription.ExternalId,
                Status = subscription.Status.ToString(),
                CollectionMethod = subscription.CollectionMethod.ToString()
            };
        }
    }
}