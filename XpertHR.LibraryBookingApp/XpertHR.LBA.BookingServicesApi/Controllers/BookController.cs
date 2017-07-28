using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
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
        //[Route("getall")]
        //[HttpGet]
        //public async Task<HttpResponseMessage> GetAll()
        //{
        //    //var allBooks = await bookRepository.GetAllAsync();
        //    //if (allBooks == null)
        //    //{
        //    //    return Request.CreateResponse(HttpStatusCode.NotFound);
        //    //}
        //    //return Request.CreateResponse(HttpStatusCode.OK, allBooks);
        //    return Request.CreateResponse(HttpStatusCode.OK)
        //}
        [Route("getall")]
        [HttpGet]
        public  HttpResponseMessage GetAll()
        {
            //var allBooks = await bookRepository.GetAllAsync();
            //if (allBooks == null)
            //{
            //    return Request.CreateResponse(HttpStatusCode.NotFound);
            //}
            //return Request.CreateResponse(HttpStatusCode.OK, allBooks);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
