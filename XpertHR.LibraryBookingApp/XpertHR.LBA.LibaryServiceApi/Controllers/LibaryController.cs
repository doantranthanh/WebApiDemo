using System;
using System.Threading.Tasks;
using System.Web.Http;
using XpertHR.LBA.ClientServices;

namespace XpertHR.LBA.LibaryServiceApi.Controllers
{
    [RoutePrefix("api/libary")]
    public class LibaryController : ApiController
    {
        private readonly IClientServicesProxy clientServicesProxy;

        public LibaryController(IClientServicesProxy clientServicesProxy)
        {
            if (clientServicesProxy == null)
                throw new ArgumentNullException(nameof(clientServicesProxy));
            this.clientServicesProxy = clientServicesProxy;
        }

        [Route("getall")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllBooks()
        {
            var allBooks = await clientServicesProxy.GetAllBookAsync();
            return Ok(allBooks);
        }


        [Route("getnotfound")]
        [HttpGet]
        public async Task<IHttpActionResult> GetNumbersTryPolicy()
        {
            var numbers = await clientServicesProxy.GetNumbersTryPolicy();
            return Ok(numbers);
        }
    }
}
