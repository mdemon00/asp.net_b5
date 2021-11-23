using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataImporter.App.Service
{
    public class HttpService : IHttpService
    {
        private HttpClient _httpClient;

        public HttpService(
            HttpClient httpClient
        )
        {
            _httpClient = httpClient;
        }
        public async Task<string> Get(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            return await sendRequest(request);
        }

        private async Task<string> sendRequest(HttpRequestMessage request)
        {
            using var response = await _httpClient.SendAsync(request);

            // throw exception on error response
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                throw new Exception(error["message"]);
            }

            string result = "";

            result = await response.Content.ReadAsStringAsync();

            return result;
        }
    }
}
