using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using XpertHR.LBA.BookingServicesApi.Handlers;

namespace XpertHR.LBA.BookingServicesApi
{
    public static class WebHandlerConfig
    {
        public static void RegisterHandlers(HttpConfiguration config)
        {
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
        }
    }
}