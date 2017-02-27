using System.Web.Mvc;

namespace DBTek.BugGuardian.TestCallerWeb.Filters
{
    public class BugGuardianFilter : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            BugGuardian.Factories.ConfigurationFactory.SetConfiguration("http://MY_TFS_SERVER:8080/Tfs", "MY_USERNAME", "MY_PASSWORD", "MY_PROJECT");
            using (var manager = new BugGuardianManager())
            {
                manager.AddBug(filterContext.Exception);
            }
        }
    }
}