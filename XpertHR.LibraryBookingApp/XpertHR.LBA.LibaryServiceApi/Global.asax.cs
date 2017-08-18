using System.Web.Http;
using Microsoft.Practices.Unity.WebApi;
using XpertHR.LBA.LibaryServiceApi.App_Start;

namespace XpertHR.LBA.LibaryServiceApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);          
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(UnityConfig.GetConfiguredContainer());
            GlobalConfiguration.Configuration.EnsureInitialized();
        }
    }
}
