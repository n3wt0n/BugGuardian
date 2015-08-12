using DBTek.BugGuardian.Extensions;
using System;
using System.Text;

namespace DBTek.BugGuardian.Helpers
{
    internal class ExceptionsHelper
    {
        public static string BuildExceptionString(Exception ex)
        {
            var exceptionString = new StringBuilder();

            //Avoid to report the wrapper exception
            if (ex.GetType().ToString().ToLower() == "system.web.httpunhandledexception" && ex.InnerException != null)
                exceptionString.Append(BuildExceptionString(ex.InnerException));
            else
            {
                exceptionString.Append($"<strong>{ex.Message.NormalizeForHtml()}</strong><br /><br />{ex.StackTrace.NormalizeForHtml()}");

                var aex = ex as AggregateException;
                if (aex != null && aex.InnerExceptions != null)
                    aex.InnerExceptions.ForEach(aexi => exceptionString.Append($"<br /><br /><i><u>Inner Exception:</u></i><br />{BuildExceptionString(aexi)}"));
                else
                {
                    //Only if not an Aggregate Exception because if it is the inner exception is printed twice
                    if (ex.InnerException != null)
                    {
                        exceptionString.Append("<br /><br /><i><u>Inner Exception:</u></i><br />");
                        exceptionString.Append(BuildExceptionString(ex.InnerException));
                    }
                }
            }

            return exceptionString.ToString();
        }

        public static string BuildExceptionTitle(Exception ex)
        {
            var title = new StringBuilder();

            if (ex.GetType().ToString().ToLower() == "system.web.httpunhandledexception" && ex.InnerException != null)
                title.Append(BuildExceptionTitle(ex.InnerException));
            else
                title.Append($"{ex.GetType().ToString()} - {ex.Source}");

            return title.ToString();
        }
    }
}
