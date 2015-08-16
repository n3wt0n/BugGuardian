using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace BugGuardian.TestCallerWeb.Filters
{
    public class BugGuardianFilter : IExceptionFilter
    {
        public bool AllowMultiple
            => true;

        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            using (var creator = new DBTek.BugGuardian.Creator())
            {
                return creator.AddBugAsync(actionExecutedContext.Exception);
            }
        }
    }
}