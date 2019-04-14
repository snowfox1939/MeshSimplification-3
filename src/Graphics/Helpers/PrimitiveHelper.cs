using OpenTK.Graphics.OpenGL;
using System;

namespace Polynano.Graphics.Helpers
{
    internal static class PrimitiveHelper
    {
        /// <summary>
        /// Get the number of vertices a primitive has
        /// </summary>
        public static int GetElementCountForPrimitive(PrimitiveType primitive)
        {
            int e;
            switch (primitive)
            {
                case PrimitiveType.Points:
                    e = 1;
                    break;
                case PrimitiveType.Lines:
                    e = 2;
                    break;
                case PrimitiveType.Triangles:
                    e = 3;
                    break;
                default:
                    throw new NotImplementedException();
            }
            return e;
        }
    }
}
