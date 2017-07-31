using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using XpertHR.LBA.BookingServicesApi.Filters;
using XpertHR.LBA.DataServices.CustomExceptions;
using XpertHR.LBA.DataServices.DataRepository;

namespace XpertHR.LBA.BookingServicesApi.Controllers
{
    [RoutePrefix("api/book")]
    public class BookController : ApiController
    {
        private readonly IBookRepository bookRepository;
        private readonly ICustomExceptionService customExceptionServices;
        static int _count = 1;

        public BookController(IBookRepository bookRepository, ICustomExceptionService customExceptionServices)
        {
            if (bookRepository == null)
                throw new ArgumentNullException(nameof(bookRepository));
            if (customExceptionServices == null)
                throw new ArgumentNullException(nameof(customExceptionServices));
            this.bookRepository = bookRepository;
            this.customExceptionServices = customExceptionServices;
        }


        [Route("getall")]
        [HttpGet]
        [ItemNotFoundExceptionFilter]
        public async Task<IHttpActionResult> GetAllBooks()
        {          
            var allBooks = await bookRepository.GetAllAsync();           
            return Ok(allBooks);         
        }


        [Route("getallavailable")]
        [HttpGet]
        [ItemNotFoundExceptionFilter]
        public async Task<IHttpActionResult> GetAllAvailableBooks()
        {
            var allBooks = await bookRepository.GetAllAvailableBooksAsync();          
            return Ok(allBooks); 
        }

        [Route("getbytitle")]
        [HttpGet]
        [ItemNotFoundExceptionFilter]
        public async Task<IHttpActionResult> GetByTitle(string title)
        {
            var allBooks = await bookRepository.GetByTitleAsync(title);
            return Ok(allBooks);
        }

        [Route("getnotfound")]
        [HttpGet]
        [ItemNotFoundExceptionFilter]
        public async Task<IHttpActionResult> GetNotFound()
        {
            await customExceptionServices.ThrowItemNotFoundException();
            return Ok();
        }

        [Route("getnullexception")]
        [HttpGet]    
        public async Task<IHttpActionResult> GetNullException()
        {
            await customExceptionServices.ThrowArgumentNullException();
            return Ok();
        }

        [Route("getnumber")]
        [HttpGet]
        public async Task<IHttpActionResult> GetNumbers()
        {
            await Task.Delay(199); 

            if (_count >= 3) 
            {
                int[] numbers = { 1, 2, 3, 4, 5 };
                _count = 1;
                return Ok(numbers);
            }
            _count++;
            return InternalServerError(new Exception("Some error"));
        }
    } 
}
