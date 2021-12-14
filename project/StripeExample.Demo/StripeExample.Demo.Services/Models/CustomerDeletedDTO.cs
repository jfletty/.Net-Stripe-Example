namespace StripeExample.Demo.Services.Models
{
    public abstract class CustomerDeletedDTO
    {
        public bool Deleted { get; set; }
        public string Id { get; set; }
    }
}