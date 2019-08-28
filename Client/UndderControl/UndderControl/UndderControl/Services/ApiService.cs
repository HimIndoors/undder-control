using Fusillade;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace UndderControl.Services
{
    public class ApiService<T> : IApiService<T>
    {
        private readonly Func<HttpMessageHandler, T> createClient;
        public ApiService(string apiBaseAddress)
        {
            createClient = messageHandler =>
            {
                var client = new HttpClient(messageHandler)
                {
                    BaseAddress = new Uri(apiBaseAddress)
                };

                return RestService.For<T>(client);
            };
        }
        private T Background
        {
            get
            {
                return new Lazy<T>(() => createClient(new RateLimitedHttpMessageHandler(new HttpClientHandler(), Priority.Background))).Value;
            }
        }
        private T UserInitiated
        {
            get
            {
                return new Lazy<T>(() => createClient(new RateLimitedHttpMessageHandler(new HttpClientHandler(), Priority.UserInitiated))).Value;
            }
        }
        private T Speculative
        {
            get
            {
                return new Lazy<T>(() => createClient(new RateLimitedHttpMessageHandler(new HttpClientHandler(), Priority.Speculative))).Value;
            }
        }
        public T GetApi(Priority priority)
        {
            switch (priority)
            {
                case Priority.Background:
                    return Background;
                case Priority.UserInitiated:
                    return UserInitiated;
                case Priority.Speculative:
                    return Speculative;
                default:
                    return UserInitiated;
            }
        }
    }
}
