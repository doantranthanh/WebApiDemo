using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using XpertHR.LBA.BookingServicesApi.Filters;
using XpertHR.LBA.DataServices.DataRepository;

namespace XpertHR.LBA.BookingServicesApi.Tests.IntegrationTests
{
    [TestFixture]
    public class WebApiIntegrationTests : IDisposable
    {
        protected HttpServer Server;
        private string _url = "http://www.xperthrlibrary.co.uk/";
       

        [SetUp]
        public void SetUpIntegrationTest()
        {           
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(name: "Default", routeTemplate: "api/{controller}/{action}/{id}", defaults: new { id = RouteParameter.Optional });
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            config.Filters.Add(new ItemNotFoundExceptionFilterAttribute());

            Server = new HttpServer(config);
        }

        protected HttpRequestMessage CreateRequest(string url, string mthv, HttpMethod method)
        {
            var request = new HttpRequestMessage();

            request.RequestUri = new Uri(_url + url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mthv));
            request.Method = method;

            return request;
        }

        public void Dispose()
        {
            if (Server != null)
            {
                Server.Dispose();
            }
        }
    }
}
