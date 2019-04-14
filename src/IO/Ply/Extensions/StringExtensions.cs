using System;
using System.Linq;

namespace Polynano.IO.Ply.Extensions 
{
    public static class StringExtensions 
    {
        /// <summary>
        /// Make sure the string contains only A-Z a-z 0-9 and _ - 
        /// </summary>
        /// <param name="val">string to test</param>
        /// <returns>whether the string is valid</returns>
        public static bool IsToken (this string val) {
            return !String.IsNullOrEmpty (val) && ((val[0] >= 'a' && val[0] <= 'z') || (val[0] >= 'A' && val[0] <= 'Z' || val[0] == '_')) &&
                val.All (c =>
                    (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') ||
                    (c >= '0' && c <= '9') || c == '_');
        }
    }
}