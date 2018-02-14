using OpenTK;
using System;

namespace Polynano.Helpers
{
    public static class GeometryMath
    {
        /// <summary>
        /// Compute the normal vector of a triangle
        /// </summary>
        /// <param name="v1">first triangle vertex</param>
        /// <param name="v2">second triangle vertex</param>
        /// <param name="v3">thrird triangle vertex</param>
        /// <returns>the normal vector</returns>
        public static Vector3 GetTriangleNormal(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            var v = v2 - v1;
            var u = v3 - v1;

            return Vector3.Cross(v, u);
        }

        /// <summary>
        /// Get the distace of a point to a plane defined by a triangle
        /// </summary>
        /// <param name="p">point to compute distance from</param>
        /// <param name="v1">triangle v1</param>
        /// <param name="v2">triangle v2</param>
        /// <param name="v3">triangle v2</param>
        /// <returns>the distance between the point and the plane</returns>
        public static float GetPointPlaneDistance(Vector3 p, Vector3 v1, Vector3 v2, Vector3 v3)
        {
            if (p == v1 || p == v2 || p == v3)
                return 0;

            var v = v2 - v1;
            var u = v3 - v1;

            var n = Vector3.Cross(v, u);
            float dist = Vector3.Dot(p - v1, n);

            return Math.Abs(dist);
        }
    }
}
