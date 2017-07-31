using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Hosting;
using FluentAssertions;
using Microsoft.Practices.Unity.WebApi;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using XpertHR.LBA.BookingServicesApi.App_Start;
using XpertHR.LBA.BookingServicesApi.Filters;
using XpertHR.LBA.BookingServicesApi.Handlers;
using XpertHR.LBA.DataServices.DataRepository;

namespace XpertHR.LBA.BookingServicesApi.Tests.IntegrationTests
{
    [TestFixture]
    public class WebApiIntegrationTests : IDisposable
    {
        protected HttpServer Server;
        protected string Url = "http://www.xperthrlibrary.co.uk/";
       

        [SetUp]
        public void SetUpIntegrationTest()
        {           
            var config = new HttpConfiguration();
            var formatters = GlobalConfiguration.Configuration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            // Web API routes        
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            config.MapHttpAttributeRoutes();
            config.Filters.Add(new ItemNotFoundExceptionFilterAttribute());
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.DependencyResolver = new UnityDependencyResolver(UnityConfig.GetConfiguredContainer());
            config.EnsureInitialized();

            Server = new HttpServer(config);
        }

        protected HttpRequestMessage CreateRequest(string url, string mthv, HttpMethod method)
        {
            var request = new HttpRequestMessage();

            request.RequestUri = new Uri(Url + url);
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
