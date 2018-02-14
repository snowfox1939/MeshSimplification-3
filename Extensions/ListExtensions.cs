using System.Collections.Generic;

namespace Polynano.Extensions
{
    public static class ListExtensions
    {
        public static void AddIfDoesNotExists<T>(this List<T> list, T val)
        {
            if(!list.Contains(val))
                list.Add(val);
        }
    }
}
