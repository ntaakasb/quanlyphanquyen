using System;
using System.Configuration;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.IO;
using Base.Common;
using Microsoft.Extensions.Options;

namespace Services
{
    public class MPortalService
    {
        private readonly IOptions<ApplicationSetting> _applicationSettings;

        private static readonly string MPortalServiceURL = null;
        private static readonly string MPortalContentType = null;
        private static readonly string MPortalBasicUsername = null;
        private static readonly string MPortalBasicPassword = null;
        private static readonly string MPortalDocumentClass = null;
        public static readonly string MPortalAppCode = null;

        //static MPortalService()
        //{
        //    MPortalServiceURL = ConfigurationManager.AppSettings["MPortalServiceURL"];
        //    if (!MPortalServiceURL.EndsWith("/"))
        //    {
        //        MPortalServiceURL += "/";
        //    }
        //    string contentType = ConfigurationManager.AppSettings["MPortalContentType"];
        //    switch (contentType)
        //    {
        //        case "application/json":
        //        case "text/json":
        //            MPortalContentType = contentType;
        //            break;
        //        default:
        //            MPortalContentType = "text/json";
        //            break;
        //    }
        //    MPortalBasicUsername = ConfigurationManager.AppSettings["MPortalBasicUsername"];
        //    MPortalBasicPassword = ConfigurationManager.AppSettings["MPortalBasicPassword"];
        //    MPortalDocumentClass = ConfigurationManager.AppSettings["MPortalDocumentClass"];
        //    MPortalAppCode = ConfigurationManager.AppSettings["MPortalAppCode"];
        //}

        public static int Call(string service, out string result, string jsonInput, string method = "POST")
        {
            string serviceURL = MPortalServiceURL + service;
            int code = _Call(serviceURL, out result, jsonInput, method);
            //MakeLog(serviceURL, jsonInput, code, result);
            return code;
        }
        public static int BasicAuthCall(string service, out string result, string appCode, string jsonInput, string method = "POST")
        {
            string serviceURL = MPortalServiceURL + service;
            int code = _BasicAuthCall(serviceURL, out result, appCode, jsonInput, method);
            return code;
        }
        private static int _BasicAuthCall(string serviceURL, out string result, string appCode, string jsonInput, string method)
        {
            try
            {
                result = string.Empty;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(serviceURL);
                httpWebRequest.ContentType = MPortalContentType;
                httpWebRequest.Method = method;
                int responseCode = 0;
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonInput);
                }
                httpWebRequest.Headers.Add("AppCode", appCode);
                httpWebRequest.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(string.Format("{0}:{1}", MPortalBasicUsername, MPortalBasicPassword))));
                try
                {
                    using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                    {
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            result = streamReader.ReadToEnd();
                        }
                        return (int)httpResponse.StatusCode;
                    }
                }
                catch (WebException e)
                {
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        responseCode = (int)httpResponse.StatusCode;
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            result = reader.ReadToEnd();
                        }
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(string.Format("Call(service={0}, jsonInput={1}, method={2}) falied!", serviceURL, jsonInput, method));
                        sb.AppendLine(string.Format("StatusCode={0}, ResponseText={1}", responseCode, result));
                        return responseCode;
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Execute " + serviceURL + " failed.", ex);
            }
        }
        private static int _Call(string serviceURL, out string result, string jsonInput, string method = "POST")
        {
            try
            {
                result = string.Empty;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(serviceURL);
                httpWebRequest.ContentType = MPortalContentType;
                httpWebRequest.Method = method;
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonInput);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                return (int)httpResponse.StatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Execute " + serviceURL + " failed.", ex);
            }
        }
        public static int AuthorizeCall(string service, string token, out string result, string appCode, string jsonInput, string method = "POST")
        {
            string serviceURL = MPortalServiceURL + service;
            int code = _AuthorizeCall(serviceURL, token, out result, appCode, jsonInput, method);
            return code;
        }
        private static int _AuthorizeCall(string serviceURL, string token, out string result, string appCode, string jsonInput, string method)
        {
            try
            {
                result = string.Empty;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(serviceURL);
                httpWebRequest.ContentType = MPortalContentType;
                httpWebRequest.Method = method;
                int responseCode = 0;
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonInput);
                }
                httpWebRequest.Headers.Add("AppCode", appCode);
                httpWebRequest.Headers.Add("Authorization", "Bearer " + token);
                try
                {
                    using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                    {
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            result = streamReader.ReadToEnd();
                        }
                        return (int)httpResponse.StatusCode;
                    }
                }
                catch (WebException e)
                {
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        responseCode = (int)httpResponse.StatusCode;
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            result = reader.ReadToEnd();
                        }
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(string.Format("Call(service={0}, jsonInput={1}, method={2}) falied!", serviceURL, jsonInput, method));
                        sb.AppendLine(string.Format("StatusCode={0}, ResponseText={1}", responseCode, result));

                        return responseCode;
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Execute " + serviceURL + " failed.", ex);
            }
        }

        private static HttpClient GetAuthenticatedHttpClient()
        {
            HttpClient httpClient = new HttpClient();
            var byteArray = Encoding.ASCII.GetBytes("admin:123456");
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            return httpClient;
        }
    }
}
