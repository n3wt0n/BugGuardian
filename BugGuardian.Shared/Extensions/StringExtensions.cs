using System;
using System.Collections.Generic;
using System.Text;

namespace DBTek.BugGuardian.Extensions
{
    public static class StringExtensions
    {
        public static string NormalizeForHtml(this string value)
            => value?.Replace(Environment.NewLine, "<br />") ?? string.Empty;        
    }
}
