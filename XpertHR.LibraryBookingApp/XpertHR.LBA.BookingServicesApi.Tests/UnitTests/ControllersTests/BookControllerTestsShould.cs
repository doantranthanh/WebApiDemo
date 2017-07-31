using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Results;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using XpertHR.LBA.BookingServicesApi.Controllers;
using XpertHR.LBA.DataServices.CustomExceptions;
using XpertHR.LBA.DataServices.DataEntities;
using XpertHR.LBA.DataServices.DataRepository;

namespace XpertHR.LBA.BookingServicesApi.Tests.UnitTests.ControllersTests
{
    [TestFixture]
    public class BookControllerTestsShould
    {
        private Mock<IBookRepository> mockBookRepository;
        private Mock<ICustomExceptionService> customExceptionSevice;
        private BookController bookController;

        [SetUp]
        public void SetUp()
        {
            mockBookRepository = new Mock<IBookRepository>();
            customExceptionSevice = new Mock<ICustomExceptionService>();
            bookController = new BookController(mockBookRepository.Object, customExceptionSevice.Object);
        }

        [Test]
        public async Task BeAbleReturnAllBooksFromDataServicesGratefully()
        {
            // Arrange
            var retrievedBooks = new List<Book>() {new Book() {Id = 1, Author = "Author1", Title = "Book1", IsBorrowed = true, Rate = 5, ReturnedDate = DateTime.Today}};
            mockBookRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(retrievedBooks);
            // Act
            using (bookController)
            {
                bookController.Request = new HttpRequestMessage();
                bookController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
                //Act
                var actionResult = await bookController.GetAllBooks();
                var getResponse = actionResult.ExecuteAsync(CancellationToken.None).Result;
                var contentResult = actionResult as OkNegotiatedContentResult<List<Book>>;
                //Assert     
                getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
                contentResult.Content.Should().NotBeNull();
                contentResult.Content[0].Id.Should().Be(1);

            }
        }

        [Test]
        public async Task BeAbleToReturnAllAvailableBooks()
        {

            // Arrange
            var retrievedBooks = new List<Book>() {new Book() {Id = 1, Author = "Author1", Title = "Book1", IsBorrowed = true, Rate = 5, ReturnedDate = DateTime.Today}};
            mockBookRepository.Setup(x => x.GetAllAvailableBooksAsync()).ReturnsAsync(retrievedBooks);
            // Act

            using (bookController)
            {
                bookController.Request = new HttpRequestMessage();
                bookController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
                //Act
                var actionResult = await bookController.GetAllAvailableBooks();
                var getResponse = actionResult.ExecuteAsync(CancellationToken.None).Result;
                var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<Book>>;
                //Assert     
                getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
                contentResult.Content.Should().NotBeNull();
                contentResult.Content.ToList()[0].Id.Should().Be(1);
            }
        }

        [Test]
        public async Task BeAbleToReturnNoContentIfNoAllAvailableBooks()
        {

            // Arrange

            mockBookRepository.Setup(x => x.GetAllAvailableBooksAsync()).ReturnsAsync(new List<Book>());
            // Act

            using (bookController)
            {
                bookController.Request = new HttpRequestMessage();
                bookController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
                //Act
                var actionResult = await bookController.GetAllAvailableBooks();
                var getResponse = actionResult.ExecuteAsync(CancellationToken.None).Result;
                //Assert                            
                getResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }
        }
    }
}
