using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XpertHR.LBA.DataServices.DataEntities;

namespace XpertHR.LBA.ClientServices
{
    public interface IClientServicesProxy
    {
        Task<HttpResponseMessage> DemoCircuitBreaker();
        Task<List<Book>> GetAllBookAsync();
        Task<string> GetNotFoundBookAsync();
        Task<IEnumerable<int>> GetNumbersTryPolicy();
    }
}