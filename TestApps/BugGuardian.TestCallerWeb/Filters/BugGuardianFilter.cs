using System.Web.Mvc;

namespace DBTek.BugGuardian.TestCallerWeb.Filters
{
    public class BugGuardianFilter : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            BugGuardian.Factories.ConfigurationFactory.SetConfiguration("myurl", "username", "password", "project");
            using (var manager = new BugGuardianManager())
            {
                manager.AddBug(filterContext.Exception);
            }
        }
    }
}