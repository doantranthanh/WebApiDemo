﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Hosting;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using XpertHR.LBA.DataServices.DataRepository;

namespace XpertHR.LBA.BookingServicesApi.Tests.IntegrationTests
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
        public void BeAbleToReturnNoContentIfNoBookReturns()
        {

            // Arrange    
            var mockExp = new ItemNotFoundException("Mock Exception");
            mockBookRepository.Setup(x => x.GetAllAsync()).ThrowsAsync(mockExp);
            // Act

            var client = new HttpClient(Server);
            var request = CreateRequest("api/books/getallbooks", "application/json", HttpMethod.Get);
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                response.Content.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }

            request.Dispose();
        }

        [Test]
        public void BeAbleReturnExceptionGratefully()
        {
            // Arrange          
            var mockExp = new ArgumentNullException("Mock Exception");
            mockBookRepository.Setup(x => x.GetAllAsync()).ThrowsAsync(mockExp);
            // Act
            var client = new HttpClient(Server);
            var request = CreateRequest("api/books/getallbooks", "application/json", HttpMethod.Get);

            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                response.Content.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            }

            request.Dispose();
        }
    }
}
