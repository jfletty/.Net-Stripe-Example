using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;

namespace StripeExample.Demo.Services.Clients
{
    public class RequestClient : IRequestClient
    {
        private readonly IRestClient _client;

        public RequestClient(
            IRestClient client)
        {
            _client = client;
        }

        public async Task<T> DoGet<T>(
            string baseUrl,
            string endpoint,
            KeyValuePair<string, string>? parameters = null,
            List<KeyValuePair<string, string>> headers = null)
            where T : class
        {
            var request = BuildRequest(endpoint, Method.GET, parameters, headers);
            return await MakeRequest<T>(baseUrl, request);
        }

        public async Task<T> DoPost<T>(
            string baseUrl,
            string endpoint,
            List<KeyValuePair<string, string>> headers = null,
            string body = null) where T : class
        {
            var request = BuildRequest(endpoint, Method.POST, headers: headers);
            return await MakeRequest<T>(baseUrl, request);
        }

        public async Task<T> DoDelete<T>(
            string baseUrl,
            string endpoint,
            KeyValuePair<string, string> parameters,
            List<KeyValuePair<string, string>> headers = null) where T : class
        {
            var request = BuildRequest(endpoint, Method.DELETE, parameters, headers);
            return await MakeRequest<T>(baseUrl, request);
        }

        private async Task<T> MakeRequest<T>(string baseUrl, IRestRequest request)
            where T : class
        {
            _client.BaseUrl = new Uri(baseUrl);
            var response =  await _client.ExecuteAsync<T>(request);
            return response.IsSuccessful ? response.Data : null;
        }
        
        private static IRestRequest BuildRequest(string endpoint,
            Method method,
            KeyValuePair<string, string>? parameter = null,
            ICollection<KeyValuePair<string, string>> headers = null,
            string body = null)
        {
            var request = new RestRequest
            {
                Resource = endpoint,
                Method = method
            };

            if (headers.Any())
            {
                request.AddHeaders(headers);
            }

            if (parameter.HasValue)
            {
                request.AddParameter(parameter.Value.Key, parameter.Value.Value);
            }

            if (!string.IsNullOrEmpty(body))
            {
                request.AddJsonBody(body);
            }

            return request;
        }
    }
}