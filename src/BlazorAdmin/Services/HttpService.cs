﻿using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorAdmin.Services
{
    public class HttpService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public HttpService(HttpClient httpClient, string apiUrl)
        {
            _httpClient = httpClient;
            _apiUrl = apiUrl;
        }

        public async Task<T> HttpGet<T>(string uri)
            where T : new()
        {
            var result = await _httpClient.GetAsync($"{_apiUrl}{uri}");
            if (!result.IsSuccessStatusCode)
            {
                return new T();
            }

            return await FromHttpResponseMessage<T>(result);
        }

        public async Task<T> HttpDelete<T>(string uri, int id)
            where T : new()
        {
            var result = await _httpClient.DeleteAsync($"{_apiUrl}{uri}/{id}");
            if (!result.IsSuccessStatusCode)
            {
                return new T();
            }

            return await FromHttpResponseMessage<T>(result);
        }

        public async Task<T> HttpPost<T>(string uri, object dataToSend)
            where T : new()
        {
            var content = ToJson(dataToSend);

            var result = await _httpClient.PostAsync($"{_apiUrl}{uri}", content);
            if (!result.IsSuccessStatusCode)
            {
                return new T();
            }

            return await FromHttpResponseMessage<T>(result);
        }

        public async Task<T> HttpPut<T>(string uri, object dataToSend)
            where T : new()
        {
            var content = ToJson(dataToSend);

            var result = await _httpClient.PutAsync($"{_apiUrl}{uri}", content);
            if (!result.IsSuccessStatusCode)
            {
                return new T();
            }

            return await FromHttpResponseMessage<T>(result);
        }


        private StringContent ToJson(object obj)
        {
            return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
        }

        private async Task<T> FromHttpResponseMessage<T>(HttpResponseMessage result)
        {
            return JsonSerializer.Deserialize<T>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
