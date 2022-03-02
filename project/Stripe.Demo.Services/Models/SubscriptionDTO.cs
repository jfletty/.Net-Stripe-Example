namespace StripeExample.Demo.Services.Models
{
    public class SubscriptionDTO
    {
        public string ExternalId { get; set; }
        public string Customer { get; set; }
        public SubscriptionStatus Status { get; set; }
        public CollectionMethod CollectionMethod { get; set; }
        // Add as many of the fields that relate to you
    }

    public enum SubscriptionStatus
    {
        trialing,
        active,
        past_due,
        canceled,
        unpaid,
        incomplete,
        incomplete_expired
    }

    public enum CollectionMethod
    {
        charge_automatically,
        send_invoice
    }
}