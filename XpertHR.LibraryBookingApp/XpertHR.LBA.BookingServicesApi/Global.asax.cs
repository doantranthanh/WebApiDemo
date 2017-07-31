using System.Web.Http;
using XpertHR.LBA.BookingServicesApi.Filters;

namespace XpertHR.LBA.BookingServicesApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(FilterApiConfig.RegisterGlobalFilters);
            GlobalConfiguration.Configure(WebApiConfig.Register);          
        }
    }
}
