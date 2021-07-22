using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using data.collection.util.DI;
using Newtonsoft.Json;

namespace data.collection.util.ApiClient
{
    public class ApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> PostAsync<T>(string apiServiceUrl, string actionName, object data = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            string result = await PostDataToRemote(apiServiceUrl, actionName, data);
            return JsonConvert.DeserializeObject<T>(result);
        }
        public async Task<T> GetAsync<T>(string apiServiceUrl, string actionName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            string result = await GetDatatoRemote(apiServiceUrl, actionName);
            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task<string> PostDataToRemote(string ApiServiceUrl, string actionName, object data)
        {
            var client = CreateHttpClient();
            string url = ApiServiceUrl + actionName;
            StringContent content = null;
            if (data != null)
            {
                var jsonData = JsonConvert.SerializeObject(data);
                content = new StringContent(jsonData);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            var response = await client.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"ApiClient发送到{url}的请求返回{response.StatusCode}");
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PostFormDatatoRemote(string ApiServiceUrl, string actionName, List<KeyValuePair<string, string>> formData)
        {
            var client = CreateHttpClient();
            string url = ApiServiceUrl + actionName;
            FormUrlEncodedContent content = new FormUrlEncodedContent(formData);

            var response = await client.PostAsync(url, content);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetDatatoRemote(string ApiServiceUrl, string actionName)
        {
            var client = CreateHttpClient();
            string url = ApiServiceUrl + actionName;

            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"ApiClient发送到{url}的请求返回{response.StatusCode}");
            }
            return await response.Content.ReadAsStringAsync();
        }

        private HttpClient CreateHttpClient()
        {
            var httpClient = _httpClientFactory.CreateClient();
            return httpClient;
        }
    }
}