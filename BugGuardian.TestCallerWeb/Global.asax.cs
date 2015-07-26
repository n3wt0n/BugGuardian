using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace BugGuardian.TestCallerWeb
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        void Application_Error(Object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            var creator = new DBTek.BugGuardian.Creator();

            Task.Run(async () =>
            {
                await creator.AddBug(ex);
            });
        }

    }
}