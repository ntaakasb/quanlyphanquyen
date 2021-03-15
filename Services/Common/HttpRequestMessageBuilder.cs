using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Services.Common
{
    public sealed class HttpRequestMessageBuilder
    {
        private readonly HttpRequestMessage _httpRequestMessage;

        private HttpRequestMessageBuilder(string url)
        {
            _httpRequestMessage = new HttpRequestMessage();
            _httpRequestMessage.RequestUri = new Uri(url);
        }

        private HttpRequestMessageBuilder(string url, HttpMethod method)
        {
            _httpRequestMessage = new HttpRequestMessage(method, url);
        }

        public static HttpRequestMessageBuilder New(string url)
        {
            return new HttpRequestMessageBuilder(url);
        }

        public static HttpRequestMessageBuilder New(string url, HttpMethod method)
        {
            return new HttpRequestMessageBuilder(url, method);
        }

        public static HttpRequestMessageBuilder NewPost(string url)
        {
            return new HttpRequestMessageBuilder(url, HttpMethod.Post);
        }

        public static HttpRequestMessageBuilder NewGet(string url)
        {
            return new HttpRequestMessageBuilder(url, HttpMethod.Get);
        }

        public static HttpRequestMessageBuilder NewDelete(string url)
        {
            return new HttpRequestMessageBuilder(url, HttpMethod.Delete);
        }

        public static HttpRequestMessageBuilder NewPut(string url)
        {
            return new HttpRequestMessageBuilder(url, HttpMethod.Put);
        }

        public static HttpRequestMessageBuilder NewPatch(string url)
        {
            return new HttpRequestMessageBuilder(url, HttpMethod.Patch);
        }

        public HttpRequestMessage Build()
        {
            return _httpRequestMessage;
        }

        public HttpRequestMessageBuilder HttpContent(HttpContent httpContent)
        {
            _httpRequestMessage.Content = httpContent;
            return this;
        }

        public HttpRequestMessageBuilder AuthorizeBearerToken(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
            }

            return this;
        }

        public HttpRequestMessageBuilder Method(HttpMethod method)
        {
            _httpRequestMessage.Method = method;
            return this;
        }
    }
}