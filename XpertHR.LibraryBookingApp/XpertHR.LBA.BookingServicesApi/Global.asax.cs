using System.Web.Http;
using Microsoft.Practices.Unity.WebApi;
using XpertHR.LBA.BookingServicesApi.App_Start;

namespace XpertHR.LBA.BookingServicesApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {         
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(FilterApiConfig.RegisterGlobalFilters);
            GlobalConfiguration.Configure(WebHandlerConfig.RegisterHandlers);
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(UnityConfig.GetConfiguredContainer());
            GlobalConfiguration.Configuration.EnsureInitialized();
        }
    }
}
