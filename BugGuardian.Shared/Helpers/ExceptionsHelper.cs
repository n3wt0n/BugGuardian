using DBTek.BugGuardian.Extensions;
using System;
using System.Text;

namespace DBTek.BugGuardian.Helpers
{
    internal class ExceptionsHelper
    {
        /// <summary>
        /// Build and return a string for the "value" of the Bug work item containing all the exception info
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string BuildExceptionString(Exception ex, string message = null)
        {
            var exceptionString = new StringBuilder();

            //Avoid to report the wrapper exception
            if (ex.GetType().ToString().ToLower() == "system.web.httpunhandledexception" && ex.InnerException != null)
                exceptionString.Append(BuildExceptionString(ex.InnerException));
            else
            {
                // Add custom message, if any
                if (!string.IsNullOrWhiteSpace(message))
                    exceptionString.Append($"<strong>{message.NormalizeForHtml()}</strong><br /><br />");

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


        /// <summary>
        /// Build and return a string used for the Title of the Bug work item
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string BuildExceptionTitle(Exception ex)
        {
            var title = new StringBuilder();

            if (ex.GetType().ToString().ToLower() == "system.web.httpunhandledexception" && ex.InnerException != null)
                title.Append(BuildExceptionTitle(ex.InnerException));
            else
                title.Append($"{ex.GetType().ToString()} - {ex.Source}");

            return title.ToString();
        }

        public static string BuildExceptionHash(Exception ex)
            => MD5CryptoServiceProvider.GetMd5String(BuildExceptionString(ex));        
    }
}
