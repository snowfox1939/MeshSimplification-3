using System;
using System.Collections.Generic;

namespace Polynano.Extensions
{
    public static class LinqExtensions
    {
        /// <summary>
        /// Find the index of a given element defined by a predicate in a list
        /// </summary>
        /// <param name="items">the list or array</param>
        /// <param name="predicate">the element to find defined as a lambda</param>
        /// <returns>the index of the element or -1 if the element was not found</returns>
        public static int FindIndex<T>(this IReadOnlyList<T> items, Func<T, bool> predicate)
        {
            var index = 0;
            foreach (var item in items)
            {
                if (predicate(item))
                    return index;
                index++;
            }
            
            return -1;
        }
    }
}
