using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

namespace StripeExample.Demo.Services.Clients
{
    public class RequestClient : IRequestClient
    {
        private readonly IRestClient _client;
        private readonly string _apiSecret;

        public RequestClient(IRestClient client, string apiSecret)
        {
            _apiSecret = apiSecret;
            _client = client;
        }

        public async Task<T> DoGet<T>(
            string endpoint,
            KeyValuePair<string, string>? parameters = null,
            List<KeyValuePair<string, string>> headers = null)
            where T : class
        {
            var request = BuildRequest(endpoint, Method.GET, parameters, headers);
            return await MakeRequest<T>(request);
        }

        public async Task<T> DoPost<T>(
            string endpoint,
            string body,
            List<KeyValuePair<string, string>> headers = null) where T : class
        {
            var request = BuildRequest(endpoint, Method.POST, null, headers, body);
            return await MakeRequest<T>(request);
        }

        public async Task<T> DoDelete<T>(
            string endpoint,
            KeyValuePair<string, string> parameters,
            List<KeyValuePair<string, string>> headers = null) where T : class
        {
            var request = BuildRequest(endpoint, Method.DELETE, parameters, headers);
            return await MakeRequest<T>(request);
        }

        private async Task<T> MakeRequest<T>(IRestRequest request)
            where T : class
        {
            var response = await _client.ExecuteAsync<T>(request);
            return response.IsSuccessful ? response.Data : null;
        }

        private IRestRequest BuildRequest(string endpoint,
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

            request.AddHeader("Authorization", $"Bearer {_apiSecret}");

            if (headers != null)
            {
                request.AddHeaders(headers);
            }

            if (parameter.HasValue)
            {
                request.AddQueryParameter(parameter.Value.Key, parameter.Value.Value);
            }

            if (!string.IsNullOrEmpty(body))
            {
                request.AddBody(body);
            }

            return request;
        }
    }
}