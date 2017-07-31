using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace XpertHR.LBA.BookingServicesApi.Tests.IntegrationTests
{
    public class WebApiKeyHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string apikey = HttpUtility.ParseQueryString(request.RequestUri.Query).Get("apikey");

            if (string.IsNullOrWhiteSpace(apikey))
            {
                return SendError("You can't use the API without the key.", HttpStatusCode.Forbidden);               
            }
            else
            {
                return base.SendAsync(request, cancellationToken);
            }
        }

        private Task<HttpResponseMessage> SendError(string error, HttpStatusCode code)
        {
            var response = new HttpResponseMessage();
            response.Content = new StringContent(error);
            response.StatusCode = code;

            return Task<HttpResponseMessage>.Factory.StartNew(() => response);
        }
    }
}
