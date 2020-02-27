using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Palindrome_Server
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var threadCountValue = ConfigurationManager.AppSettings["ThreadCount"];
            if (int.TryParse(threadCountValue, out var threadCount))
            {
                if(threadCount < 1 || threadCount > 10)
                    throw new ArgumentException("Количество потоков должно быть от 1 до 10", nameof(threadCount));
            }
            else throw new ArgumentException("Количество потоков должно быть целым числом", nameof(threadCount));
        }
    }
}
