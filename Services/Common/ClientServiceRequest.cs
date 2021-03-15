using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Base.Common;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace Services.Common
{
    public abstract class ClientServiceRequest
    {
     
        private readonly JsonSerializer _jsonSerializer = new JsonSerializer();

        /// <summary>Gets or sets the service base URI.</summary>
        public abstract string BaseUri
        {
            get; set;
        }

        public abstract string RestPath
        {
            get; set;
        }

        public abstract int Timeout
        {
            get; set;
        }

        private HttpClient httpClient;

        public ClientServiceRequest()
        {
        }

        public void Init()
        {
            var api = ApiSetting.Apis.FirstOrDefault(x => x.Address == BaseUri);
            httpClient = api != null ? null : InitHttpClientWithBaseUri(BaseUri);
        }

        private HttpClient InitHttpClientWithBaseUri(string baseUri)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseUri);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        }

        #region SEND
        protected async Task<Stream> SendAsync(HttpRequestMessage requestMessage)
        {
            using (var response = await HttpClientWithAuthorizaton().SendAsync(requestMessage))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStreamAsync();
            }
        }

        protected async Task<TResult> SendAsync<TResult>(HttpRequestMessage requestMessage)
        {
            using (var response = await HttpClientWithAuthorizaton().SendAsync(requestMessage))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                var jsonTextReader = new JsonTextReader(new StreamReader(stream));
                var answer = _jsonSerializer.Deserialize<TResult>(jsonTextReader);
                return answer;
            }
        }
        #endregion

        private static string AddQueryParamToUrl(string path, object obj)
        {
            var result = path;
            if (obj == null)
            {
                return result;
            }

            var objType = obj.GetType();
            if (objType.IsGenericType)
            {
                var enumerable = obj as IEnumerable;
                if (enumerable != null)
                {
                    var inputParam = new Dictionary<string, string>();
                    foreach (var param in enumerable)
                    {
                        inputParam.Add(objType.Name, param.ToString());
                    }

                    result = QueryHelpers.AddQueryString(result, inputParam);
                }
            }
            else
            {
                var props = objType.GetProperties();

                foreach (var prop in props)
                {
                    var value = prop.GetValue(obj);
                    if (value != null)
                    {
                        var inputParam = new Dictionary<string, string>();
                        if (prop.PropertyType.IsGenericType)
                        {
                            var enumerable = value as IEnumerable;
                            if (enumerable != null)
                            {
                                foreach (var item in enumerable)
                                {
                                    if (item != null)
                                    {
                                        inputParam.Add(prop.Name, item.ToString());
                                    }
                                }
                            }
                            else
                            {
                                inputParam.Add(prop.Name, value.ToString());
                            }
                        }
                        else
                        {
                            inputParam.Add(prop.Name, value.ToString());
                        }

                        result = QueryHelpers.AddQueryString(result, inputParam);
                    }
                }
            }

            return result;
        }

        public T Get<T>(string action = null, string token = null)
            where T : new()
        {
            var uri = new Uri(new Uri(BaseUri), action);
            var url = uri.AbsoluteUri;

            var requestMessage = HttpRequestMessageBuilder
                .NewGet(url)
                .AuthorizeBearerToken(token)
                .Build();
            return SendAsync<T>(requestMessage).Result;
        }

        public async Task<T> GetAsync<T>(string action = null, string token = null, IEnumerable<KeyValuePair<string, object>> queryParams = null)
            where T : Result, new()
        {
            var uri = new Uri(new Uri(BaseUri), action);
            var fullPath = uri.AbsoluteUri;
            // append query parameters to request
            if (queryParams != null && queryParams.Any())
            {
                var inputParam = new Dictionary<string, string>();
                foreach (var param in queryParams)
                {
                    if (param.Value != null)
                    {
                        inputParam.Add(param.Key, param.Value.ToString());
                    }
                }

                fullPath = QueryHelpers.AddQueryString(uri.AbsoluteUri, inputParam);
            }

            var requestMessage = HttpRequestMessageBuilder
                .NewGet(fullPath)
                .AuthorizeBearerToken(token)
                .Build();
            return await SendAsync<T>(requestMessage);
        }

        public async Task<T> Get<T, V>(V value, string action = null, string token = null)
            where T : Result, new()
        {
            var uri = new Uri(new Uri(BaseUri), action);
            var fullPath = AddQueryParamToUrl(uri.AbsoluteUri, value);

            var requestMessage = HttpRequestMessageBuilder
                .NewGet(fullPath)
                .AuthorizeBearerToken(token)
                .Build();
            return await SendAsync<T>(requestMessage);
        }

        public async Task<T> GetDataSourceRequestQueryStringAsync<T>(string queryString, string action = null, string token = null)
           where T : Result, new()
        {
            var uri = new Uri(new Uri(BaseUri), action + queryString);
            var requestMessage = HttpRequestMessageBuilder
                .NewGet(uri.AbsoluteUri)
                .AuthorizeBearerToken(token)
                .Build();
            return await SendAsync<T>(requestMessage);
        }

        public async Task<TResult> GetAsync<TResult, TParam>(TParam queryParams, string action = null, string token = null)
            where TResult : Result, new()
        {
            var uri = new Uri(new Uri(BaseUri), action);
            var fullPath = AddQueryParamToUrl(uri.AbsoluteUri, queryParams);

            var requestMessage = HttpRequestMessageBuilder
                .NewGet(fullPath)
                .AuthorizeBearerToken(token)
                .Build();
            return await SendAsync<TResult>(requestMessage);
        }

        public async Task<string> GetRequestURIAsync(string action = null, string token = null)
        {
            var uri = new Uri(new Uri(BaseUri), action);
            var url = uri.AbsoluteUri;

            var requestMessage = HttpRequestMessageBuilder
                .NewGet(url)
                .AuthorizeBearerToken(token)
                .Build();

            using (var response = await HttpClientWithAuthorizaton().SendAsync(requestMessage))
            {
                return response.RequestMessage.RequestUri.ToString();
            }
        }

        public async Task<byte[]> GetDataStreamFromUrlAsync(string action = null, string token = null)
        {
            var uri = new Uri(new Uri(BaseUri), action);
            var url = uri.AbsoluteUri;

            var requestMessage = HttpRequestMessageBuilder
                .NewGet(url)
                .AuthorizeBearerToken(token)
                .Build();

            using (var response = await HttpClientWithAuthorizaton().SendAsync(requestMessage))
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
        }

        #region Post
        protected async Task<string> PostAsync(string url, HttpContent httpContent, string authorizeBearerToken = null)
        {
            var requestMessage = HttpRequestMessageBuilder
                .NewPost(url)
                .HttpContent(httpContent)
                .AuthorizeBearerToken(authorizeBearerToken)
                .Build();
            return new StreamReader(await SendAsync(requestMessage)).ReadToEnd();
        }

        protected async Task<TResult> PostAsync<TResult>(string url, HttpContent httpContent, string authorizeBearerToken = null)
        {
            var requestMessage = HttpRequestMessageBuilder
                .NewPost(url)
                .HttpContent(httpContent)
                .AuthorizeBearerToken(authorizeBearerToken)
                .Build();
            return await SendAsync<TResult>(requestMessage);
        }

        public T Post<T, V>(V value, string action = null, string token = null)
            where T : new()
        {
            using (var content = new StringContent(JsonConvert.SerializeObject(value), System.Text.Encoding.UTF8, "application/json"))
            {
                var uri = new Uri(new Uri(BaseUri), action);
                var fullPath = uri.AbsoluteUri;
                return PostAsync<T>(fullPath, content, token).Result;
            }
        }

        public async Task<T> PostAsync<T, V>(V value, string action = null, string token = null)
            where T : Result, new()
        {
            using (var content = new StringContent(JsonConvert.SerializeObject(value), System.Text.Encoding.UTF8, "application/json"))
            {
                var uri = new Uri(new Uri(BaseUri), action);
                var fullPath = uri.AbsoluteUri;
                return await PostAsync<T>(fullPath, content, token);
            }
        }

        public async Task<T> PostDynamicAsync<T>(string action = null, string token = null)
          where T : new()
        {
            var uri = new Uri(new Uri(BaseUri), action);
            var fullPath = uri.AbsoluteUri;
            var requestMessage = HttpRequestMessageBuilder
                .NewPut(fullPath)
                .AuthorizeBearerToken(token)
                .Build();
            return await SendAsync<T>(requestMessage);
        }
        #endregion

        #region Put
        protected async Task<string> PutAsync(string url, HttpContent httpContent, string authorizeBearerToken = null)
        {
            var requestMessage = HttpRequestMessageBuilder
                .NewPut(url)
                .HttpContent(httpContent)
                .AuthorizeBearerToken(authorizeBearerToken)
                .Build();
            return new StreamReader(await SendAsync(requestMessage)).ReadToEnd();
        }

        protected async Task<TResult> PutAsync<TResult>(string url, HttpContent httpContent, string authorizeBearerToken = null)
        {
            var requestMessage = HttpRequestMessageBuilder
                .NewPut(url)
                .HttpContent(httpContent)
                .AuthorizeBearerToken(authorizeBearerToken)
                .Build();
            return await SendAsync<TResult>(requestMessage);
        }

        public async Task<T> PutAsync<T>(string action = null, string token = null)
            where T : Result, new()
        {
            var uri = new Uri(new Uri(BaseUri), action);
            var fullPath = uri.AbsoluteUri;
            var requestMessage = HttpRequestMessageBuilder
                .NewPut(fullPath)
                .AuthorizeBearerToken(token)
                .Build();
            return await SendAsync<T>(requestMessage);
        }

        public async Task<T> PutAsync<T, V>(V value, string action = null, string token = null)
            where T : Result, new()
        {
            using (var content = new StringContent(JsonConvert.SerializeObject(value), System.Text.Encoding.UTF8, "application/json"))
            {
                var uri = new Uri(new Uri(BaseUri), action);
                var fullPath = uri.AbsoluteUri;
                return await PutAsync<T>(fullPath, content, token);
            }
        }

        public async Task<T> PutDynamicAsync<T>(string action = null, string token = null)
            where T : new()
        {
            var uri = new Uri(new Uri(BaseUri), action);
            var fullPath = uri.AbsoluteUri;
            var requestMessage = HttpRequestMessageBuilder
                .NewPut(fullPath)
                .AuthorizeBearerToken(token)
                .Build();
            return await SendAsync<T>(requestMessage);
        }

        public async Task<T> PutDynamicByteAsync<T>(ByteArrayContent value, string action = null, string token = null)
            where T : new()
        {
            var uri = new Uri(new Uri(BaseUri), action);
            var fullPath = uri.AbsoluteUri;
            return await PutAsync<T>(fullPath, value, token);
        }
        #endregion

        #region Delete
        public T Delete<T>(string action = null, string token = null)
            where T : Result, new()
        {
            var uri = new Uri(new Uri(BaseUri), action);
            var fullPath = uri.AbsoluteUri;
            var requestMessage = HttpRequestMessageBuilder
                .NewDelete(fullPath)
                .AuthorizeBearerToken(token)
                .Build();
            return SendAsync<T>(requestMessage).Result;
        }

        public async Task<T> DeleteAsync<T>(string action = null, string token = null)
            where T : Result, new()
        {
            var uri = new Uri(new Uri(BaseUri), action);
            var fullPath = uri.AbsoluteUri;
            var requestMessage = HttpRequestMessageBuilder
                .NewDelete(fullPath)
                .AuthorizeBearerToken(token)
                .Build();
            return await SendAsync<T>(requestMessage);
        }

        public async Task DeleteAsync(string action = null, string token = null)
        {
            var uri = new Uri(new Uri(BaseUri), action);
            var fullPath = uri.AbsoluteUri;
            var requestMessage = HttpRequestMessageBuilder
                .NewDelete(fullPath)
                .AuthorizeBearerToken(token)
                .Build();
            using (var response = await HttpClientWithAuthorizaton().SendAsync(requestMessage))
            {
                response.EnsureSuccessStatusCode();
            }
        }

        public T Delete<T, V>(V value, string action = null, string token = null) where T : Result, new()
        {
            var uri = new Uri(new Uri(BaseUri), action);
            var fullPath = uri.AbsoluteUri;
            var content = new StringContent(JsonConvert.SerializeObject(value), System.Text.Encoding.UTF8, "application/json");

            var requestMessage = HttpRequestMessageBuilder
                .NewDelete(fullPath)
                .AuthorizeBearerToken(token)
                .HttpContent(content)
                .Build();
            return SendAsync<T>(requestMessage).Result;
        }

        public async Task<T> DeleteAsync<T, V>(V value, string action = null, string token = null)
            where T : Result, new()
        {
            var uri = new Uri(new Uri(BaseUri), action);
            var fullPath = uri.AbsoluteUri;
            var content = new StringContent(JsonConvert.SerializeObject(value), System.Text.Encoding.UTF8, "application/json");
            var requestMessage = HttpRequestMessageBuilder
                .NewDelete(fullPath)
                .AuthorizeBearerToken(token)
                .HttpContent(content)
                .Build();
            return await SendAsync<T>(requestMessage);
        }
        #endregion

        public HttpClient RestClientWithAuthorizaton()
        {
            var api = ApiSetting.Apis.FirstOrDefault(x => x.Address == BaseUri);
            if (api != null)
            {
                switch (api.EndpointCode)
                {
                    case ApiCode.ApiServices:
                        httpClient = HttpClientConfSingleton.GetInstanceWithToken(BaseUri).HttpClient;
                        break;
                    default:
                        httpClient = new HttpClient();
                        httpClient.BaseAddress = new Uri(BaseUri);
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        break;
                }
            }
            else
            {
                httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(BaseUri);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }

            return httpClient;
        }

        public HttpClient HttpClientWithAuthorizaton()
        {
            var api = ApiSetting.Apis.FirstOrDefault(x => x.Address == BaseUri);
            if (api != null)
            {
                switch (api.EndpointCode)
                {
                    case ApiCode.ApiServices:
                        httpClient = HttpClientAuthenSingleton.GetInstanceWithToken(BaseUri).HttpClient;
                        break;
                    default:
                        httpClient = InitHttpClientWithBaseUri(BaseUri);
                        break;
                }
            }
            else
            {
                httpClient = InitHttpClientWithBaseUri(BaseUri);
            }

            return httpClient;
        }

        private static bool IsOfNullableType<T>(T o)
        {
            var type = typeof(T);
            return Nullable.GetUnderlyingType(type) != null;
        }
    }
}