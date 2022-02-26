using System;
namespace StripeExample.Demo.Services.Models
{
    public class CustomerDTO
    {
        public string ExternalId { get; set; }
        public long Balance { get; set; }
        public DateTime Created { get; set; }
        public string Currency { get; set; }
        public bool? Deleted { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }

        // Add as many of the fields that relate to you
    }
}