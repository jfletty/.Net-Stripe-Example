using System;
using AutoFixture;
using Stripe;
using StripeExample.Demo.Services.Helpers;
using StripeExample.Demo.Services.Models;
using Xunit;

namespace StripeExample.Demo.UnitTests.StripeDemo.Services.Helpers
{
    public class SubscriberConverterTests
    {
        private readonly Fixture _fixture = new();
        
        [Fact]
        public void WhenStripeSubscriberIsReceived_ElementsAreConvertedToSubscriberDTOCorrectly()
        {
            // arrange
            var original = new Subscription()
            {
                Id = _fixture.Create<string>(),
                Status = _fixture.Create<SubscriptionStatus>().ToString(),
                CollectionMethod = _fixture.Create<CollectionMethod>().ToString()
            };
            
            // act
            var result = SubscriptionConverter.Convert(original);
            
            // assert
            Assert.Equal(original.Id, result.ExternalId);
            Assert.Equal(original.Status, result.Status.ToString());
            Assert.Equal(original.CollectionMethod, result.CollectionMethod.ToString());
        }
        
        [Fact]
        public void WhenCustomerDTOIsReceived_ElementsAreConvertedToStripeCustomerCorrectly()
        {
            // arrange
            var original = _fixture.Create<SubscriptionDTO>();
            
            // act
            var result = SubscriptionConverter.Convert(original);
            
            // assert
            Assert.Equal(original.ExternalId, result.Id);
            Assert.Equal(original.Status, Enum.Parse<SubscriptionStatus>(result.Status));
            Assert.Equal(original.CollectionMethod, Enum.Parse<CollectionMethod>(result.CollectionMethod));
        }
    }
}