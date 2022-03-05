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
using StripeExample.Demo.Services.Services.Interfaces;
using Xunit;

namespace StripeExample.Demo.UnitTests.StripeDemo.Services.Services;

public class ManageSubscriptionTests
{
    private readonly Fixture _fixture = new();
    private readonly IManageSubscription _manageSubscription;
    private readonly string _subscriptionId;
    private readonly Mock<IRequestClient> _requestClient;

    public ManageSubscriptionTests()
    {
        _subscriptionId = _fixture.Create<string>();
        var config = new StripeConfig
        {
            Url = _fixture.Create<Uri>().ToString(),
            StripeSecret = _fixture.Create<string>()
        };
        _requestClient = new Mock<IRequestClient>();
        _manageSubscription = new ManageSubscription(config)
        {
            RequestClient = _requestClient.Object
        };
    }
    
    [Fact]
    public async Task WhenDoGetSuccessfullyReturnsSubscription_SubscriptionIsConvertedAndReturned()
    {
        // arrange
        var original = new Subscription
        {
            Id = _fixture.Create<string>(),
            Status = _fixture.Create<SubscriptionStatus>().ToString(),
            CollectionMethod = _fixture.Create<CollectionMethod>().ToString()
        };
        
        var parameters = new KeyValuePair<string, string>("", _subscriptionId);

        _requestClient
            .Setup(x => x.DoGet<Subscription>("subscriptions", parameters, null))
            .ReturnsAsync(original);
        
        // act
        var result = await _manageSubscription.GetAsync(_subscriptionId);
        
        // assert
        Assert.Equal(original.Id, result.ExternalId);
        Assert.Equal(original.Status, result.Status.ToString());
        Assert.Equal(original.CollectionMethod, result.CollectionMethod.ToString());
    }
    
    [Fact]
    public async Task WhenDoGetSuccessfullyReturnsNull_NullIsReturned()
    {
        // arrange
        var parameters = new KeyValuePair<string, string>("", _subscriptionId);

        _requestClient
            .Setup(x => x.DoGet<Subscription>("subscriptions", parameters, null))
            .ReturnsAsync((Subscription)null);
        
        // act
        var result = await _manageSubscription.GetAsync(_subscriptionId);
        
        // assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task WhenDoGetThrowAnException_ExceptionIsRethrownFromGetAsync()
    {
        // arrange
        var parameters = new KeyValuePair<string, string>("", _subscriptionId);

        _requestClient
            .Setup(x => x.DoGet<Subscription>("subscriptions", parameters, null))
            .Throws<HttpRequestException>();
        
        // act & assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _manageSubscription.GetAsync(_subscriptionId));
    }
    
    [Fact]
    public async Task WhenDoGetSuccessfullyReturnsSubscriptions_SubscriptionsAreConvertedAndReturned()
    {
        // arrange
        var original = new List<Subscription>
        {
            new()
            {
                Id = _fixture.Create<string>(),
                Status = _fixture.Create<SubscriptionStatus>().ToString(),
                CollectionMethod = _fixture.Create<CollectionMethod>().ToString()
            }
        };

        _requestClient
            .Setup(x => x.DoGet<List<Subscription>>("subscriptions", null, null))
            .ReturnsAsync(original);
        
        // act
        var result = await _manageSubscription.GetAllAsync();
        
        // assert
        Assert.Equal(original[0].Id, result[0].ExternalId);
        Assert.Equal(original[0].Status, result[0].Status.ToString());
        Assert.Equal(original[0].CollectionMethod, result[0].CollectionMethod.ToString());
    }
    
    [Fact]
    public async Task WhenDoGetSuccessfullyReturnsNullForManySubscriptions_NullIsReturned()
    {
        // arrange
        _requestClient
            .Setup(x => x.DoGet<List<Subscription>>("subscriptions", null, null))
            .ReturnsAsync((List<Subscription>)null);
        
        // act
        var result = await _manageSubscription.GetAsync(_subscriptionId);
        
        // assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task WhenDoGetThrowAnException_ExceptionIsRethrownFromGetAllAsync()
    {
        // arrange
        _requestClient
            .Setup(x => x.DoGet<List<Subscription>>("subscriptions", null, null))
            .Throws<HttpRequestException>();
        
        // act & assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _manageSubscription.GetAllAsync());
    }
    
    [Fact]
    public async Task WhenDoPostSuccessfullyCreatesSubscription_SubscriptionIsConvertedAndReturned()
    {
        // arrange
        var original = new Subscription
        {
            Id = _fixture.Create<string>(),
            Status = _fixture.Create<SubscriptionStatus>().ToString(),
            CollectionMethod = _fixture.Create<CollectionMethod>().ToString()
        };
        
        var expected = SubscriptionConverter.Convert(original);
        
        _requestClient
            .Setup(x => x.DoPost<Subscription>("subscriptions", It.IsAny<string>(), null))
            .ReturnsAsync(original);
        
        // act
        var result = await _manageSubscription.CreateOrUpdateAsync(expected);
        
        // assert
        Assert.Equal(original.Id, result.ExternalId);
        Assert.Equal(original.Status, result.Status.ToString());
        Assert.Equal(original.CollectionMethod, result.CollectionMethod.ToString());
    }
    
    [Fact]
    public async Task WhenDoPostThrowAnException_ExceptionIsRethrown()
    {
        // arrange
        var subscription = _fixture.Create<SubscriptionDTO>();
        
        _requestClient
            .Setup(x => x.DoPost<Subscription>("subscriptions", It.IsAny<string>(), null))
            .Throws<HttpRequestException>();
        
        // act & assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _manageSubscription.CreateOrUpdateAsync(subscription));
    }
}