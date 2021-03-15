using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Services.Common
{
    public class HttpClientConfSingleton
    {
        public HttpClient HttpClient;
        private static HttpClientConfSingleton httpClientSingleTonToken = null;
        HttpClientConfSingleton(string baseUrl)
        {
            HttpClient = new HttpClient();
            HttpClient.BaseAddress = new Uri(baseUrl);
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static HttpClientConfSingleton GetInstanceWithToken(string baseUrl)
        {
            if (httpClientSingleTonToken == null)
            {
                httpClientSingleTonToken = new HttpClientConfSingleton(baseUrl);
            }

            return httpClientSingleTonToken;
        }
    }

    public class HttpClientAssessSingleton
    {
        public HttpClient HttpClient;
        private static HttpClientAssessSingleton httpClientSingleTonToken = null;

        HttpClientAssessSingleton(string baseUrl)
        {
            HttpClient = new HttpClient();
            HttpClient.BaseAddress = new Uri(baseUrl);
            HttpClient.Timeout = TimeSpan.FromSeconds(200);
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static HttpClientAssessSingleton GetInstanceWithToken(string baseUrl)
        {
            if (httpClientSingleTonToken == null)
            {
                httpClientSingleTonToken = new HttpClientAssessSingleton(baseUrl);
            }

            return httpClientSingleTonToken;
        }
    }

    public class HttpClientAuthenSingleton
    {
        public HttpClient HttpClient;
        private static HttpClientAuthenSingleton httpClientSingleTon = null;
        private static HttpClientAuthenSingleton httpClientSingleTonToken = null;


        public HttpClientAuthenSingleton()
        {
        }

        HttpClientAuthenSingleton(string baseUrl)
        {
            HttpClient = new HttpClient();
            HttpClient.BaseAddress = new Uri(baseUrl);
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static HttpClientAuthenSingleton GetInstance(string baseUrl)
        {
            if (httpClientSingleTon == null)
            {
                httpClientSingleTon = new HttpClientAuthenSingleton(baseUrl);
            }

            return httpClientSingleTon;
        }

        public static HttpClientAuthenSingleton GetInstanceWithToken(string baseUrl)
        {
            if (httpClientSingleTonToken == null)
            {
                httpClientSingleTonToken = new HttpClientAuthenSingleton(baseUrl);
            }

            return httpClientSingleTonToken;
        }
    }

    public class HttpClientEmpSingleton
    {
        public HttpClient HttpClient;
        private static HttpClientEmpSingleton httpClientSingleTonToken = null;
        HttpClientEmpSingleton(string baseUrl)
        {
            HttpClient = new HttpClient();
            HttpClient.BaseAddress = new Uri(baseUrl);
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static HttpClientEmpSingleton GetInstanceWithToken(string baseUrl)
        {
            if (httpClientSingleTonToken == null)
            {
                httpClientSingleTonToken = new HttpClientEmpSingleton(baseUrl);
            }

            return httpClientSingleTonToken;
        }
    }

    public class HttpClientTalSingleton
    {
        public HttpClient HttpClient;
        private static HttpClientTalSingleton httpClientSingleTonToken = null;
        HttpClientTalSingleton(string baseUrl)
        {
            HttpClient = new HttpClient();
            HttpClient.BaseAddress = new Uri(baseUrl);
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static HttpClientTalSingleton GetInstanceWithToken(string baseUrl)
        {
            if (httpClientSingleTonToken == null)
            {
                httpClientSingleTonToken = new HttpClientTalSingleton(baseUrl);
            }

            return httpClientSingleTonToken;
        }
    }

    public class HttpClientReportingSingleton
    {
        public HttpClient HttpClient;
        private static HttpClientReportingSingleton httpClientSingleTonToken = null;
        HttpClientReportingSingleton(string baseUrl)
        {
            HttpClient = new HttpClient();
            HttpClient.BaseAddress = new Uri(baseUrl);
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static HttpClientReportingSingleton GetInstanceWithToken(string baseUrl)
        {
            if (httpClientSingleTonToken == null)
            {
                httpClientSingleTonToken = new HttpClientReportingSingleton(baseUrl);
            }

            return httpClientSingleTonToken;
        }
    }
}