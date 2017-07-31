using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace XpertHR.LBA.BookingServicesApi.Handlers
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            if (context.Exception is ArgumentNullException)
            {
                var result = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(context.Exception.Message),
                    ReasonPhrase = "ArgumentNullException"
                };

                context.Result = new ArgumentNullResult(context.Request, result);
            }
            else
            {
                // Handle other exceptions, do other things
            }
        }

        private class ArgumentNullResult : IHttpActionResult
        {
            private HttpRequestMessage request;
            private readonly HttpResponseMessage httpResponseMessage;


            public ArgumentNullResult(HttpRequestMessage request, HttpResponseMessage httpResponseMessage)
            {
                this.request = request;
                this.httpResponseMessage = httpResponseMessage;
            }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(httpResponseMessage);
            }
        }
    }
}