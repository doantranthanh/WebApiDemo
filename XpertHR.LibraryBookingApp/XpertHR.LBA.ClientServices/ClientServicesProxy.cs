using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using XpertHR.LBA.DataServices.DataEntities;

namespace XpertHR.LBA.ClientServices
{
    public class ClientServicesProxy : IClientServicesProxy
    {
        private static HttpClient client = new HttpClient();
        private static RetryPolicy<HttpResponseMessage> httpRetryPolicy;
        private static CircuitBreakerPolicy circuitBreakerPolicy;

        private static HttpStatusCode[] httpStatusCodesWorthRetrying = {
            HttpStatusCode.NotFound, // 404
            HttpStatusCode.InternalServerError, // 500
            HttpStatusCode.BadRequest, // 400
        };

        public ClientServicesProxy()
        {
            client.BaseAddress = new Uri("http://localhost:57622/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpRetryPolicy = Policy
                .Handle<HttpResponseException>()
                .OrResult<HttpResponseMessage>(r => ((IList) httpStatusCodesWorthRetrying).Contains(r.StatusCode))
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt));

            circuitBreakerPolicy = Policy
                .Handle<HttpResponseException>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: 2,
                    durationOfBreak: TimeSpan.FromSeconds(10)
                );
        }

        public async Task<List<Book>> GetAllBookAsync()
        {
            List<Book> books = null;
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress + "api/book/getall");
            if (response.IsSuccessStatusCode)
            {
                books = await response.Content.ReadAsAsync<List<Book>>();
            }
            return books;
        }

        public async Task<string> GetNotFoundBookAsync()
        {
            var response = await httpRetryPolicy.ExecuteAsync(() => client.GetAsync(client.BaseAddress + "api/book/getnotfound"));
            return response.ReasonPhrase;
        }

        public async Task<IEnumerable<int>> GetNumbersTryPolicy()
        {

            var httpResponse = await httpRetryPolicy.ExecuteAsync(() => client.GetAsync(client.BaseAddress + "api/book/getnumber"));

            var numbersTried = await httpResponse.Content.ReadAsAsync<IEnumerable<int>>();
            return numbersTried;
        }

        public async Task<HttpResponseMessage> DemoCircuitBreaker()
        {
            var response = await httpRetryPolicy.ExecuteAsync(() => circuitBreakerPolicy.ExecuteAsync(() => client.GetAsync(client.BaseAddress + "api/book/getexceptions"), true));
            return response;
        }
    }
}
