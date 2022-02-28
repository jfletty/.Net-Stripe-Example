using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using StripeExample.Demo.Services.Clients;
using StripeExample.Demo.Services.Models;
using Xunit;

namespace StripeExample.Demo.UnitTests.StripeDemo.Services.Clients
{
    public class RequestClientTests
    {
        private readonly RequestClient _requestClient;
        private readonly Mock<IRestClient> _restClient;
        private readonly Fixture _fixture;

        public RequestClientTests()
        {
            _fixture = new Fixture();
            _restClient = new Mock<IRestClient>();
            _requestClient = new RequestClient(_fixture.Create<Uri>().ToString(), "");
        }

        [Fact]
        public async Task WhenDoGetIsCalled_WithRequiredFields_RequestIsSuccessful()
        {
            // arrange
            var restResponse = BuildCustomerResponse();

            _restClient
                .Setup(x => x.ExecuteAsync<CustomerDTO>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(restResponse);

            // act
            var result = await _requestClient.DoGet<CustomerDTO>("getrequest");

            // assert
            Assert.Equal(restResponse.Data, result);
        }

        [Fact]
        public async Task WhenDoGetIsCalled_WithQueryParameters_ParametersAreSuccessfullyAdded()
        {
            // arrange
            List<Parameter> actualParameters = null;

            var restResponse = BuildCustomerResponse();
            var parameter = new KeyValuePair<string, string>(_fixture.Create<string>(), _fixture.Create<string>());

            _restClient
                .Setup(x => x.ExecuteAsync<CustomerDTO>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
                .Callback<IRestRequest, CancellationToken>(
                    (request, _) => { actualParameters = request.Parameters; })
                .ReturnsAsync(restResponse);

            // act
            var result = await _requestClient.DoGet<CustomerDTO>("getrequest", parameter);

            // assert
            Assert.Equal(restResponse.Data, result);
            Assert.NotNull(actualParameters);
            Assert.Equal(parameter.Key, actualParameters[0].Name);
            Assert.Equal(parameter.Value, actualParameters[0].Value);
            Assert.Equal(ParameterType.QueryString, actualParameters[0].Type);
        }

        [Fact]
        public async Task WhenDoGetIsCalled_WithHeaders_HeadersAreSuccessfullyAdded()
        {
            // arrange
            List<Parameter> actualHeaders = null;

            var restResponse = BuildCustomerResponse();
            var headers = new List<KeyValuePair<string, string>>
            {
                new(_fixture.Create<string>(), _fixture.Create<string>()),
                new(_fixture.Create<string>(), _fixture.Create<string>())
            };

            _restClient
                .Setup(x => x.ExecuteAsync<CustomerDTO>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
                .Callback<IRestRequest, CancellationToken>(
                    (request, _) => { actualHeaders = request.Parameters; })
                .ReturnsAsync(restResponse);

            // act
            var result = await _requestClient.DoGet<CustomerDTO>("getrequest", headers: headers);

            // assert
            Assert.Equal(restResponse.Data, result);

            Assert.NotNull(actualHeaders);
            Assert.Equal(headers[0].Key, actualHeaders[0].Name);
            Assert.Equal(headers[0].Value, actualHeaders[0].Value);
            Assert.Equal(ParameterType.HttpHeader, actualHeaders[0].Type);

            Assert.Equal(headers[1].Key, actualHeaders[1].Name);
            Assert.Equal(headers[1].Value, actualHeaders[1].Value);
            Assert.Equal(ParameterType.HttpHeader, actualHeaders[1].Type);
        }

        [Fact]
        public async Task WhenAnyMethodTypeIsCalledAndErrorIsThrown_ErrorIsReturnedToTheClient()
        {
            var exception = new HttpRequestException("Bad Request", null, HttpStatusCode.BadRequest);
            // arrange
            _restClient
                .Setup(x => x.ExecuteAsync<CustomerDTO>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            // act & assert
            await Assert.ThrowsAsync<HttpRequestException>(() =>
                _requestClient.DoGet<CustomerDTO>("getrequest"));
            await Assert.ThrowsAsync<HttpRequestException>(() =>
                _requestClient.DoPost<CustomerDTO>("postrequest", null));
            await Assert.ThrowsAsync<HttpRequestException>(() => _requestClient.DoDelete<CustomerDTO>("deleterequest",
                new KeyValuePair<string, string>(_fixture.Create<string>(),
                    _fixture.Create<string>())));
        }

        [Fact]
        public async Task WhenDoPostIsCalled_WithRequiredFields_RequestIsSuccessful()
        {
            // arrange
            List<Parameter> actualBody = null;
            var restResponse = BuildCustomerResponse();
            var restRequest = new RestRequest
            {
                Method = Method.POST,
                Resource = "postmethod",
                Body = new RequestBody("", "", JsonConvert.SerializeObject(restResponse.Data))
            };

            _restClient
                .Setup(x => x.ExecuteAsync<CustomerDTO>(It.IsAny<IRestRequest>(), It.IsAny<CancellationToken>()))
                .Callback<IRestRequest, CancellationToken>(
                    (request, _) => { actualBody = request.Parameters; })
                .ReturnsAsync(restResponse);

            // act
            var result =
                await _requestClient.DoPost<CustomerDTO>(restRequest.Resource,
                    restRequest.Body.Value.ToString());

            // assert
            Assert.Equal(restResponse.Data, result);
            Assert.Equal(restRequest.Body.Value, actualBody[0].Value);
            Assert.Equal(ParameterType.RequestBody, actualBody[0].Type);
        }

        [Fact]
        public async Task WhenDoPostIsCalled_WithHeaders_HeadersAreSuccessfullyAdded()
        {
            // arrange
            List<Parameter> actualHeaders = null;
            var restResponse = BuildCustomerResponse();

            var restRequest = new RestRequest
            {
                Method = Method.POST,
                Resource = "postmethod",
                Body = new RequestBody("", "", JsonConvert.SerializeObject(restResponse.Data))
            };

            var headers = new List<KeyValuePair<string, string>>
            {
                new(_fixture.Create<string>(), _fixture.Create<string>()),
                new(_fixture.Create<string>(), _fixture.Create<string>())
            };

            _restClient
                .Setup(x => x.ExecuteAsync<CustomerDTO>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
                .Callback<IRestRequest, CancellationToken>(
                    (request, _) =>
                    {
                        actualHeaders = request.Parameters.Where(x => x.Type == ParameterType.HttpHeader).ToList();
                    })
                .ReturnsAsync(restResponse);

            // act
            var result = await _requestClient.DoPost<CustomerDTO>(restRequest.Resource,
                restRequest.Body.Value.ToString(), headers);

            // assert
            Assert.Equal(restResponse.Data, result);

            Assert.NotNull(actualHeaders);
            Assert.Equal(headers[0].Key, actualHeaders[0].Name);
            Assert.Equal(headers[0].Value, actualHeaders[0].Value);
            Assert.Equal(ParameterType.HttpHeader, actualHeaders[0].Type);

            Assert.Equal(headers[1].Key, actualHeaders[1].Name);
            Assert.Equal(headers[1].Value, actualHeaders[1].Value);
            Assert.Equal(ParameterType.HttpHeader, actualHeaders[1].Type);
        }

        [Fact]
        public async Task WhenDoDeleteIsCalled_WithRequiredFields_RequestIsSuccessful()
        {
            // arrange
            List<Parameter> actualParameters = null;
            var restResponse = BuildCustomerResponse();
            var parameter = new KeyValuePair<string, string>(_fixture.Create<string>(), _fixture.Create<string>());

            _restClient
                .Setup(x => x.ExecuteAsync<CustomerDTO>(It.IsAny<IRestRequest>(), It.IsAny<CancellationToken>()))
                .Callback<IRestRequest, CancellationToken>(
                    (request, _) => { actualParameters = request.Parameters; })
                .ReturnsAsync(restResponse);

            // act
            var result = await _requestClient.DoDelete<CustomerDTO>("dodelete", parameter);

            // assert
            Assert.Equal(restResponse.Data, result);
            Assert.Equal(parameter.Key, actualParameters[0].Name);
            Assert.Equal(parameter.Value, actualParameters[0].Value);
            Assert.Equal(ParameterType.QueryString, actualParameters[0].Type);
        }

        [Fact]
        public async Task WhenDoDeleteIsCalled_WithHeaders_HeadersAreSuccessfullyAdded()
        {
            // arrange
            List<Parameter> actualHeaders = null;
            var restResponse = BuildCustomerResponse();
            var parameter = new KeyValuePair<string, string>(_fixture.Create<string>(), _fixture.Create<string>());

            var headers = new List<KeyValuePair<string, string>>
            {
                new(_fixture.Create<string>(), _fixture.Create<string>()),
                new(_fixture.Create<string>(), _fixture.Create<string>())
            };

            _restClient
                .Setup(x => x.ExecuteAsync<CustomerDTO>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
                .Callback<IRestRequest, CancellationToken>(
                    (request, _) =>
                    {
                        actualHeaders = request.Parameters.Where(x => x.Type == ParameterType.HttpHeader).ToList();
                    })
                .ReturnsAsync(restResponse);

            // act
            var result = await _requestClient.DoDelete<CustomerDTO>("doDelete", parameter, headers);

            // assert
            Assert.Equal(restResponse.Data, result);

            Assert.NotNull(actualHeaders);
            Assert.Equal(headers[0].Key, actualHeaders[0].Name);
            Assert.Equal(headers[0].Value, actualHeaders[0].Value);
            Assert.Equal(ParameterType.HttpHeader, actualHeaders[0].Type);

            Assert.Equal(headers[1].Key, actualHeaders[1].Name);
            Assert.Equal(headers[1].Value, actualHeaders[1].Value);
            Assert.Equal(ParameterType.HttpHeader, actualHeaders[1].Type);
        }

        private IRestResponse<CustomerDTO> BuildCustomerResponse(bool isSuccessful = true)
        {
            var customer = _fixture.Create<CustomerDTO>();

            return new RestResponse<CustomerDTO>
            {
                ResponseStatus = isSuccessful ? ResponseStatus.Completed : ResponseStatus.Error,
                StatusCode = isSuccessful ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
                Data = customer
            };
        }
    }
}