using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using XpertHR.LBA.DataServices.DataEntities;

namespace XpertHR.LBA.BookingClientApp
{
    class Program
    {
        private static HttpClient client = new HttpClient();
        private static RetryPolicy<HttpResponseMessage> httpRetryPolicy;


        private static HttpStatusCode[] httpStatusCodesWorthRetrying = {
            HttpStatusCode.NotFound, // 404
            HttpStatusCode.InternalServerError, // 500
            HttpStatusCode.BadRequest, // 400
        };

        private static CircuitBreakerPolicy circuitBreakerPolicy;

        static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("http://localhost:57622/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpRetryPolicy = Policy
                                    .Handle<HttpResponseException>()
                                    .OrResult<HttpResponseMessage>(r => httpStatusCodesWorthRetrying.Contains(r.StatusCode))
                                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt));

            circuitBreakerPolicy = Policy
                .Handle<HttpResponseException>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: 2,
                    durationOfBreak: TimeSpan.FromSeconds(10)
                );

            try
            {
                var books = await GetAllBookAsync(client.BaseAddress + "api/book/getall");
                foreach (var book in books)
                {
                    Console.WriteLine(book.Title);
                }

                var notfoundPhrase = await GetNotFoundBookAsync(client.BaseAddress + "api/book/getnotfound");
                Console.WriteLine(notfoundPhrase);

                var numbers = await GetNumbersTryPolicy(client.BaseAddress + "api/book/getnumber");
                foreach (var number in numbers)
                {
                    Console.WriteLine(number);
                }


                var circuitPhaseResponse = await DemoCircuitBreaker(client.BaseAddress + "api/book/getexceptions");
                Console.WriteLine(circuitPhaseResponse.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }

        

        static async Task<List<Book>> GetAllBookAsync(string requestEndpoint)
        {
            List<Book> books = null;
            HttpResponseMessage response = await client.GetAsync(requestEndpoint);
            if (response.IsSuccessStatusCode)
            {
                books = await response.Content.ReadAsAsync<List<Book>>();
            }
            return books;
        }

        static async Task<string> GetNotFoundBookAsync(string requestEndpoint)
        {
            var response = await httpRetryPolicy.ExecuteAsync(() => client.GetAsync(requestEndpoint));           
            return response.ReasonPhrase;
        }

        static async Task<IEnumerable<int>> GetNumbersTryPolicy(string requestEndpoint)
        {

            var httpResponse = await httpRetryPolicy.ExecuteAsync(() => client.GetAsync(requestEndpoint));

            var numbersTried = await httpResponse.Content.ReadAsAsync<IEnumerable<int>>();
            return numbersTried;
        }

        static async Task<HttpResponseMessage> DemoCircuitBreaker(string requestEndpoint)
        {
            var response = await httpRetryPolicy.ExecuteAsync(() => circuitBreakerPolicy.ExecuteAsync(() => client.GetAsync(requestEndpoint), true));
            return response;
        }
    }
}
