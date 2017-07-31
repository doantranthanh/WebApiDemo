using System.Net.Http;
using System.Web.Http;
using MvcRouteTester;
using NUnit.Framework;
using XpertHR.LBA.BookingServicesApi.Controllers;

namespace XpertHR.LBA.BookingServicesApi.Tests.UnitTests.WebApiRoutingTests
{
    [TestFixture]
    public class BookControllerRouteTestsShould
    {
        private HttpConfiguration _httpConfiguration;

        [SetUp]
        public void SetUp()
        {
            _httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(_httpConfiguration);
            _httpConfiguration.EnsureInitialized();
        }

        [Test]
        public void BeAbleGetAllBookWithCorrectRouteCallAppropirateMethod()
        {
            const string route = "/api/books/getall";
            RouteAssert.HasApiRoute(_httpConfiguration, route, HttpMethod.Get);
            _httpConfiguration.ShouldMap(route).To<BookController>(HttpMethod.Get, x => x.GetAllBooks());
        }

        [Test]
        public void BeAbleGetAllAvailableBookWithCorrectRouteCallAppropirateMethod()
        {
            const string route = "/api/books/getallavailable";
            RouteAssert.HasApiRoute(_httpConfiguration, route, HttpMethod.Get);
            _httpConfiguration.ShouldMap(route).To<BookController>(HttpMethod.Get, x => x.GetAllAvailableBooks());
        }
    }
}
