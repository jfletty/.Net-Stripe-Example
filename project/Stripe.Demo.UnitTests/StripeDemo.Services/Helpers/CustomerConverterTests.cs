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
            var original = new Customer
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
            var result = CustomerConverter.Convert(original);
            
            // assert
            Assert.Equal(original.Id, result.ExternalId);
            Assert.Equal(original.Balance, result.Balance);
            Assert.Equal(original.Currency, result.Currency);
            Assert.Equal(original.Deleted, result.Deleted);
            Assert.Equal(original.Description, result.Description);
            Assert.Equal(original.Email, result.Email);
            Assert.Equal(original.Name, result.Name);
            Assert.Equal(original.Phone, result.Phone);
        }
        
        [Fact]
        public void WhenCustomerIsNull_NullIsReturned()
        {
            // arrange
            Customer original = null;
            
            // act
            var result = CustomerConverter.Convert(original);
            
            // assert
            Assert.Null(result);
        }
        
        [Fact]
        public void WhenCustomerDTOIsReceived_ElementsAreConvertedToStripeCustomerCorrectly()
        {
            // arrange
            var original = _fixture.Create<CustomerDTO>();
            
            // act
            var result = CustomerConverter.Convert(original);
            
            // assert
            Assert.Equal(original.ExternalId, result.Id);
            Assert.Equal(original.Balance, result.Balance);
            Assert.Equal(original.Currency, result.Currency);
            Assert.Equal(original.Deleted, result.Deleted);
            Assert.Equal(original.Description, result.Description);
            Assert.Equal(original.Email, result.Email);
            Assert.Equal(original.Name, result.Name);
            Assert.Equal(original.Phone, result.Phone);
        }
        
        [Fact]
        public void WhenCustomerDTOIsNull_NullIsReturned()
        {
            // arrange
            CustomerDTO original = null;
            
            // act
            var result = CustomerConverter.Convert(original);
            
            // assert
            Assert.Null(result);
        }
    }
}