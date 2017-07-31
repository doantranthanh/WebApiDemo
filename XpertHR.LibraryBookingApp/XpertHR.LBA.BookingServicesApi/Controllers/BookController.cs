using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using XpertHR.LBA.DataServices.DataRepository;

namespace XpertHR.LBA.BookingServicesApi.Controllers
{
    [RoutePrefix("api/books")]
    public class BookController : ApiController
    {
        private readonly IBookRepository bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            if (bookRepository == null)
                throw new ArgumentNullException(nameof(bookRepository));
            this.bookRepository = bookRepository;
        }


        [Route("getall")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllBooks()
        {
            try
            {
                var allBooks = await bookRepository.GetAllAsync();
                if (!allBooks.Any())
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
                return Ok(allBooks);
            }
            catch (ArgumentNullException ex)
            {
                return InternalServerError(ex);
            }
        }


        [Route("getallavailable")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllAvailableBooks()
        {
            var allBooks = await bookRepository.GetAllAvailableBooksAsync();
            if (!allBooks.ToList().Any())
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            return Ok(allBooks); 
        }
    }
}
