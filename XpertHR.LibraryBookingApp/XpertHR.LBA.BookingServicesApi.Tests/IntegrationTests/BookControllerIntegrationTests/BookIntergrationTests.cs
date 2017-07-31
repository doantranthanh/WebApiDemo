using System;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using XpertHR.LBA.DataServices.DataRepository;

namespace XpertHR.LBA.BookingServicesApi.Tests.IntegrationTests.BookControllerIntegrationTests
{
    [TestFixture]
    public class BookIntergrationTests : WebApiIntegrationTests
    {
        private Mock<IBookRepository> mockBookRepository;

        [SetUp]
        public void InitTest()
        {
            mockBookRepository = new Mock<IBookRepository>();          
        }


        [Test]
        public void BeAbleToReturnAllBooks()
        {

            // Arrange    
           
            // Act

            var client = new HttpClient(Server);
            var request = CreateRequest("api/book/getall", "application/json", HttpMethod.Get);
            using (var response = client.SendAsync(request).Result)
            {
                response.Content.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }

            request.Dispose();
        }


        [Test]
        public void BeAbleToReturnNotFoundExceptionThroughExceptionFilter()
        {

            // Arrange    

            // Act

            var client = new HttpClient(Server);
            var request = CreateRequest("api/book/getnotfound", "application/json", HttpMethod.Get);
            using (var response = client.SendAsync(request).Result)
            {
                response.Content.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.NotFound);
                response.ReasonPhrase.Should().Be("ItemNotFound");
            }

            request.Dispose();
        }


        [Test]
        public void BeAbleToReturnArgumentNullExceptionThroughGlobalExceptionHandler()
        {

            // Arrange    

            // Act

            var client = new HttpClient(Server);
            var request = CreateRequest("api/book/getnullexception", "application/json", HttpMethod.Get);
            using (var response = client.SendAsync(request).Result)
            {
                response.Content.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
                response.ReasonPhrase.Should().Be("ArgumentNullException");
            }

            request.Dispose();
        }

        [Test]
        public void BeAbleToReturnArgumentNullExceptionThroughGlobalExceptionHandlerWithGetTitle()
        {

            // Arrange    

            // Act

            var client = new HttpClient(Server);
            var request = CreateRequest("api/book/getbytitle?title", "application/json", HttpMethod.Get);
            using (var response = client.SendAsync(request).Result)
            {
                response.Content.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
                response.ReasonPhrase.Should().Be("ArgumentNullException");
            }

            request.Dispose();
        }
    }
}
