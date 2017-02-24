using System;
using System.Collections.Generic;

namespace DBTek.BugGuardian.Extensions
{
    internal static class CollectionExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                if (item != null)
                    action(item);
            }

            return source;
        }
    }
}
