using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using XpertHR.LBA.BookingServicesApi.Controllers;
using XpertHR.LBA.DataServices.DataEntities;
using XpertHR.LBA.DataServices.DataRepository;

namespace XpertHR.LBA.BookingServicesApi.Tests.UnitTests.ControllersTests
{
    [TestFixture]
    public class BookControllerTestsShould
    {
        private BookController sut;
        private Mock<IBookRepository> bookRepository;
        [SetUp]
        public void SetUp()
        {
            bookRepository = new Mock<IBookRepository>();
            sut = new BookController(bookRepository.Object);
        }

        [Test]
        public async Task BeAbleReturnAllBooksInSystemGratefully()
        {
            // Arrange
            bookRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Book>());
            // Act
            var response = await sut.GetAll();
            // Assert
            Assert.AreEqual("OK", response.StatusCode.ToString());

        }
    }
}
