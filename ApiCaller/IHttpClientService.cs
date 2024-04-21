using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ApiCaller
{
    public interface IHttpClientService
    {
        Task<HttpResponseMessage> GetAsync(string url, Dictionary<string, string> headers = null, string authorizationToken = null);
        Task<HttpResponseMessage> PostAsync(string url, HttpContent content, Dictionary<string, string> headers = null, string authorizationToken = null);
        Task<HttpResponseMessage> DeleteAsync(string url, Dictionary<string, string> headers = null, string authorizationToken = null);
    }

    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;

        public HttpClientService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> GetAsync(string url, Dictionary<string, string> headers = null, string authorizationToken = null)
        {
            ConfigureHttpClient(headers, authorizationToken);
            return await _httpClient.GetAsync(url);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content, Dictionary<string, string> headers = null, string authorizationToken = null)
        {
            ConfigureHttpClient(headers, authorizationToken);
            return await _httpClient.PostAsync(url, content);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url, Dictionary<string, string> headers = null, string authorizationToken = null)
        {
            ConfigureHttpClient(headers, authorizationToken);
            return await _httpClient.DeleteAsync(url);
        }

        private void ConfigureHttpClient(Dictionary<string, string> headers, string authorizationToken)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            if (!string.IsNullOrEmpty(authorizationToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorizationToken);
            }
        }
    }
}
