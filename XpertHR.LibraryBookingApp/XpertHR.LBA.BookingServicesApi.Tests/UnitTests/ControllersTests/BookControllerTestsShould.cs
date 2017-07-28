using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;
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
 
        private Mock<IBookRepository> bookRepository;
        [SetUp]
        public void SetUp()
        {
            bookRepository = new Mock<IBookRepository>();         
        }

        [Test]
        public async Task BeAbleReturnAllBooksInSystemGratefully()
        {
            // Arrange
            bookRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Book>());
            // Act
            using (var controller = new BookController(bookRepository.Object))
            {
                controller.Request = new HttpRequestMessage();
                controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
                //Act
                var response = await controller.GetAll();

                //IEnumerable<Product> products;
                ////Assert
                //Assert.IsTrue(response.TryGetContentValue(out products));
                ////Assert
                //Assert.AreEqual(5, products.Count());

                Assert.AreEqual("OK", response.StatusCode.ToString());

            }
        }
    }
}
