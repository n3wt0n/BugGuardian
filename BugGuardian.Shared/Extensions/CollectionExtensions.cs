using System;
using System.Collections.Generic;
using System.Text;

namespace DBTek.BugGuardian.Extensions
{
    public static class CollectionExtensions
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
