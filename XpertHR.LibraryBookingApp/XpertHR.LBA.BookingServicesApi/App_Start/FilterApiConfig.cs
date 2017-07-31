using System.Web.Http;
using XpertHR.LBA.BookingServicesApi.Filters;

namespace XpertHR.LBA.BookingServicesApi
{
    public static class FilterApiConfig
    {
        public static void RegisterGlobalFilters(HttpConfiguration config)
        {
            config.Filters.Add(new ItemNotFoundExceptionFilterAttribute());
        }
    }
}