using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace XpertHR.LBA.BookingServicesApi.Controllers
{
    [RoutePrefix("api/books")]
    public class BookController : ApiController
    {
        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
