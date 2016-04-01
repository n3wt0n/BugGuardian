using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace DBTek.BugGuardian.TestCallerWeb.Filters
{
    public class BugGuardianFilter : HandleErrorAttribute
    {        
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            using (var creator = new DBTek.BugGuardian.Creator())
            {
                creator.AddBug(filterContext.Exception);
            }
        }
    }
}