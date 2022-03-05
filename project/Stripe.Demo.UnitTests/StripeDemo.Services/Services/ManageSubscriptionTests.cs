using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using Stripe;
using StripeExample.Demo.Services.Clients;
using StripeExample.Demo.Services.Helpers;
using StripeExample.Demo.Services.Models;
using StripeExample.Demo.Services.Services;
using Xunit;

namespace StripeExample.Demo.UnitTests.StripeDemo.Services.Services;

public class ManageCustomerTests
{
    private readonly Fixture _fixture = new();
    private readonly ManageCustomer _manageCustomer;
    private readonly string _customerId;
    private readonly Mock<IRequestClient> _requestClient;

    public ManageCustomerTests()
    {
        _customerId = _fixture.Create<string>();
        var config = new StripeConfig
        {
            Url = _fixture.Create<Uri>().ToString(),
            StripeSecret = _fixture.Create<string>()
        };
        _requestClient = new Mock<IRequestClient>();
        _manageCustomer = new ManageCustomer(config)
        {
            RequestClient = _requestClient.Object
        };
    }
    
    [Fact]
    public async Task WhenDoGetSuccessfullyReturnsCustomer_CustomerIsConvertedAndReturned()
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
        
        var parameters = new KeyValuePair<string, string>("", _customerId);

        _requestClient
            .Setup(x => x.DoGet<Customer>("customers", parameters, null))
            .ReturnsAsync(original);
        
        // act
        var result = await _manageCustomer.GetAsync(_customerId);
        
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
    public async Task WhenDoGetSuccessfullyReturnsNull_NullIsReturned()
    {
        // arrange
        var parameters = new KeyValuePair<string, string>("", _customerId);

        _requestClient
            .Setup(x => x.DoGet<Customer>("customers", parameters, null))
            .ReturnsAsync((Customer)null);
        
        // act
        var result = await _manageCustomer.GetAsync(_customerId);
        
        // assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task WhenDoGetThrowAnException_ExceptionIsRethrownFromGetAsync()
    {
        // arrange
        var parameters = new KeyValuePair<string, string>("", _customerId);

        _requestClient
            .Setup(x => x.DoGet<Customer>("customers", parameters, null))
            .Throws<HttpRequestException>();
        
        // act & assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _manageCustomer.GetAsync(_customerId));
    }
    
    [Fact]
    public async Task WhenDoGetSuccessfullyReturnsCustomers_CustomersAreConvertedAndReturned()
    {
        // arrange
        var original = new List<Customer>
        {
            new()
            {
                Id = _fixture.Create<string>(),
                Balance = _fixture.Create<long>(),
                Currency = _fixture.Create<string>(),
                Deleted = _fixture.Create<bool>(),
                Description = _fixture.Create<string>(),
                Email = _fixture.Create<string>(),
                Name = _fixture.Create<string>(),
                Phone = _fixture.Create<string>()
            }
        };

        _requestClient
            .Setup(x => x.DoGet<List<Customer>>("customers", null, null))
            .ReturnsAsync(original);
        
        // act
        var result = await _manageCustomer.GetAllAsync();
        
        // assert
        Assert.Equal(original[0].Id, result[0].ExternalId);
        Assert.Equal(original[0].Balance, result[0].Balance);
        Assert.Equal(original[0].Currency, result[0].Currency);
        Assert.Equal(original[0].Deleted, result[0].Deleted);
        Assert.Equal(original[0].Description, result[0].Description);
        Assert.Equal(original[0].Email, result[0].Email);
        Assert.Equal(original[0].Name, result[0].Name);
        Assert.Equal(original[0].Phone, result[0].Phone);
    }
    
    [Fact]
    public async Task WhenDoGetSuccessfullyReturnsNullForManyCustomers_NullIsReturned()
    {
        // arrange
        _requestClient
            .Setup(x => x.DoGet<List<Customer>>("customers", null, null))
            .ReturnsAsync((List<Customer>)null);
        
        // act
        var result = await _manageCustomer.GetAsync(_customerId);
        
        // assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task WhenDoGetThrowAnException_ExceptionIsRethrownFromGetAllAsync()
    {
        // arrange
        _requestClient
            .Setup(x => x.DoGet<List<Customer>>("customers", null, null))
            .Throws<HttpRequestException>();
        
        // act & assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _manageCustomer.GetAllAsync());
    }
    
    [Fact]
    public async Task WhenDoPostSuccessfullyCreatesCustomer_CustomerIsConvertedAndReturned()
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
        
        var expected = CustomerConverter.Convert(original);
        
        _requestClient
            .Setup(x => x.DoPost<Customer>("customers", It.IsAny<string>(), null))
            .ReturnsAsync(original);
        
        // act
        var result = await _manageCustomer.CreateOrUpdateCustomerAsync(expected);
        
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
    public async Task WhenDoPostThrowAnException_ExceptionIsRethrown()
    {
        // arrange
        var customer = _fixture.Create<CustomerDTO>();
        
        _requestClient
            .Setup(x => x.DoPost<Customer>("customers", It.IsAny<string>(), null))
            .Throws<HttpRequestException>();
        
        // act & assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _manageCustomer.CreateOrUpdateCustomerAsync(customer));
    }
    
    [Fact]
    public async Task WhenDoDeleteSuccessfullyReturnsDeletes_TrueIsReturned()
    {
        // arrange
        var parameters = new KeyValuePair<string, string>("", _customerId);

        _requestClient
            .Setup(x => x.DoDelete<List<Customer>>("customers", parameters, null));
        
        // act
        var result = await _manageCustomer.DeleteCustomerAsync(_customerId);
        
        // assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task WhenDoDeleteThrowAnException_ExceptionIsRethrown()
    {
        var parameters = new KeyValuePair<string, string>("", _customerId);

        // arrange
        _requestClient
            .Setup(x => x.DoDelete<CustomerDeletedDTO>("customers", parameters, null))
            .Throws<HttpRequestException>();
        
        // act & assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _manageCustomer.DeleteCustomerAsync(_customerId));
    }
}