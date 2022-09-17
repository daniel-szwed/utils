using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ArbreSoft.Utils
{
    public class ApiClient : IDisposable
    {
        private CookieContainer cookies;
        private readonly HttpClient client;
        private HttpClientHandler handler;

        public ApiClient()
        {
            handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            cookies = new CookieContainer();
            handler.CookieContainer = cookies;
            client = new HttpClient(handler);
        }
        
        public IEnumerable<Cookie> GetCookies()
        {
            return cookies.GetCookies(client.BaseAddress).Cast<Cookie>();
        }

        public void Dispose()
        {
            client.Dispose();
        }

        public ApiClient SetBaseAddress(string baseAddress)
        {
            client.BaseAddress = new Uri(baseAddress);
            return this;
        }

        public ApiClient SetCookie(Cookie cookie)
        {
            cookies.Add(cookie);
            return this;
        }

        public Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage message)
        {
            return client.SendAsync(message);
        }

        public ApiClient SetTimeout(int miliseconds)
        {
            client.Timeout = TimeSpan.FromMilliseconds(miliseconds);
            return this;
        }

        public ApiClient SkipCertificateValidation()
        {
            handler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            return this;
        }
    }
}