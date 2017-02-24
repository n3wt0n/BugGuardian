using System.Web.Mvc;

namespace DBTek.BugGuardian.TestCallerWeb.Filters
{
    public class BugGuardianFilter : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            using (var manager = new BugGuardianManager())
            {
                manager.AddBug(filterContext.Exception);
            }
        }
    }
}