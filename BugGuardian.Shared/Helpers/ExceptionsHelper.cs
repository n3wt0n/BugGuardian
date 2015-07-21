using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBTek.BugGuardian.Extensions;

namespace DBTek.BugGuardian.Helpers
{
    public class ExceptionsHelper
    {
        public static string BuildExceptionString(Exception ex)
        {
            var exceptionString = new StringBuilder();
            exceptionString.AppendFormat("<strong>{0}</strong><br /><br />{1}", ex.Message, (ex.StackTrace ?? string.Empty).Replace(Environment.NewLine, "<br />"));

            if (ex.InnerException != null)
            {
                exceptionString.Append("<br /><br /><i><u>Inner Exception:</u></i><br />");
                exceptionString.Append(BuildExceptionString(ex.InnerException));
            }

            var aex = ex as AggregateException;
            if (aex != null && aex.InnerExceptions != null)
                aex.InnerExceptions.ForEach(aexi => exceptionString.Append(aexi));                                       

            return exceptionString.ToString();
        }
    } 
}
