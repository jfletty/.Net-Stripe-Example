using AutoFixture;
using Stripe;
using StripeExample.Demo.Services.Helpers;
using StripeExample.Demo.Services.Models;
using Xunit;

namespace StripeExample.Demo.UnitTests.StripeDemo.Services.Helpers
{
    public class CustomerConverterTests
    {
        private readonly Fixture _fixture = new();
        
        [Fact]
        public void WhenStripeCustomerIsReceived_ElementsAreConvertedToCustomerDTOCorrectly()
        {
            // arrange
            var orignal = new Customer
            {
                Id = _fixture.Create<string>(),
                Balance = _fixture.Create<long>(),
                Currency = _fixture.Create<string>(),
                Deleted = _fixture.Create<bool>(),
                Description = _fixture.Create<string>(),
                Email = _fixture.Create<string>(),
                Name = _fixture.Create<string>(),
                Phone = _fixture.Create<string>()
            };
            
            // act
            var result = CustomerConverter.Convert(orignal);
            
            // assert
            Assert.Equal(orignal.Id, result.ExternalId);
            Assert.Equal(orignal.Balance, result.Balance);
            Assert.Equal(orignal.Currency, result.Currency);
            Assert.Equal(orignal.Deleted, result.Deleted);
            Assert.Equal(orignal.Description, result.Description);
            Assert.Equal(orignal.Email, result.Email);
            Assert.Equal(orignal.Name, result.Name);
            Assert.Equal(orignal.Phone, result.Phone);
        }
        
        [Fact]
        public void WhenCustomerDTOIsReceived_ElementsAreConvertedToStripeCustomerCorrectly()
        {
            // arrange
            var orignal = _fixture.Create<CustomerDTO>();
            
            // act
            var result = CustomerConverter.Convert(orignal);
            
            // assert
            Assert.Equal(orignal.ExternalId, result.Id);
            Assert.Equal(orignal.Balance, result.Balance);
            Assert.Equal(orignal.Currency, result.Currency);
            Assert.Equal(orignal.Deleted, result.Deleted);
            Assert.Equal(orignal.Description, result.Description);
            Assert.Equal(orignal.Email, result.Email);
            Assert.Equal(orignal.Name, result.Name);
            Assert.Equal(orignal.Phone, result.Phone);
        }
    }
}