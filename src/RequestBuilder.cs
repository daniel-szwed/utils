using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;

namespace ArbreSoft.Utils
{
    public class RequestBuilder
    {
        private HttpRequestMessage requestMessage;

        public RequestBuilder()
        {
            requestMessage = new HttpRequestMessage();
        }

        public RequestBuilder AddFileToFormData(byte[] file, string name, string fileName)
        {
            if(requestMessage.Content == null)
            {
                requestMessage.Content = new MultipartFormDataContent();
            }
            ((MultipartFormDataContent)requestMessage.Content).Add(new StreamContent(new MemoryStream(file)), name, fileName);
            return this;
        }

        public RequestBuilder AddHeader(HttpRequestHeader header, string value)
        {
            requestMessage.Headers.Add(header.ToString(), value);
            return this;
        }

        public RequestBuilder AddHeader(string header, string value)
        {
            requestMessage.Headers.Add(header, value);
            return this;
        }

        public RequestBuilder AddKeyToFormData(string key, string value)
        {
            if (requestMessage.Content == null)
            {
                requestMessage.Content = new MultipartFormDataContent();
            }

            ((MultipartFormDataContent)requestMessage.Content).Add(new StringContent(value), key); 
            return this;
        }

        public HttpRequestMessage Build()
        {
            return requestMessage;
        }

        public RequestBuilder SetFormUrlEncodedContent(IEnumerable<KeyValuePair<string, string>> content)
        {
            requestMessage.Content = new FormUrlEncodedContent(content);
            return this;
        }
        
        public RequestBuilder SetMethod(HttpMethod method)
        {
            requestMessage.Method = method;
            return this;
        }

        public RequestBuilder SetStringContent(string content, Encoding encoding, string mediaType)
        {
            requestMessage.Content = new StringContent(content, encoding, mediaType);
            return this;
        }
        
        public RequestBuilder SetUri(string uri)
        {
            requestMessage.RequestUri = new Uri(uri);
            return this;
        }
        public RequestBuilder AddQueryParameters(Dictionary<string, string> parameters)
        {
            var sb = new StringBuilder();
            foreach (var kvp in parameters)
            {
                if (sb.Length > 0) { sb.Append("&"); }
                sb.AppendFormat("{0}={1}",
                    HttpUtility.UrlEncode(kvp.Key),
                    HttpUtility.UrlEncode(kvp.Value));
            }

            requestMessage.RequestUri = new Uri($"{requestMessage.RequestUri}?{sb}");
            return this;
        }

        
    }
}