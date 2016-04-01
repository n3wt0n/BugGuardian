using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace DBTek.BugGuardian.TestCallerWeb
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup                                
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        //OPTION 2: global handler
        //void Application_Error(Object sender, EventArgs e)
        //{
        //    Exception ex = Server.GetLastError();
        //    using (var creator = new DBTek.BugGuardian.Creator())
        //    {
        //        creator.AddBugAsync(ex);
        //    }
        //}
    }
}