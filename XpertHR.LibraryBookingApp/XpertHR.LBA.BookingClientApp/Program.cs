using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Polly;
using Polly.Retry;
using XpertHR.LBA.DataServices.DataEntities;

namespace XpertHR.LBA.BookingClientApp
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static RetryPolicy<HttpResponseMessage> httpRequestPolicy;

        static HttpStatusCode[] httpStatusCodesWorthRetrying = {
            HttpStatusCode.NotFound, // 404
            HttpStatusCode.InternalServerError, // 500
            HttpStatusCode.BadRequest, // 400
        };
        static void Main(string[] args)
        {         
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("http://localhost:57622/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpRequestPolicy = Policy
                                    .Handle<HttpResponseException>()
                                    .OrResult<HttpResponseMessage>(r => httpStatusCodesWorthRetrying.Contains(r.StatusCode))
                                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt));
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
            var response = await httpRequestPolicy.ExecuteAsync(() => client.GetAsync(requestEndpoint));         
            return response.ReasonPhrase;
        }

        static async Task<IEnumerable<int>> GetNumbersTryPolicy(string requestEndpoint)
        {

            var httpResponse = await httpRequestPolicy.ExecuteAsync(() => client.GetAsync(requestEndpoint));

            var numbersTried = await httpResponse.Content.ReadAsAsync<IEnumerable<int>>();
            return numbersTried;
        }
    }
}
