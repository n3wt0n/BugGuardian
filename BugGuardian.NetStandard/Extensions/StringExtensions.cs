using System;

namespace DBTek.BugGuardian.Extensions
{
    public static class StringExtensions
    {
        internal static string NormalizeForHtml(this string value)
            => value?.Replace(Environment.NewLine, "<br />") ?? string.Empty;        
    }
}
