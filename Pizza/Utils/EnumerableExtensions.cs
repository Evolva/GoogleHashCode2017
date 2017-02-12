using System;
using System.Collections.Generic;

namespace Pizza.Utils
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> actionWithIndex)
        {
            var index = 0;
            foreach (var item in source)
            {
                actionWithIndex(item, index);
                index += 1;
            }
        }
    }
}