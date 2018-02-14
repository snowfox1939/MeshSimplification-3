using OpenTK;
using System;

namespace Polynano.IO
{
    internal class VertexNormalizer
    {
        /// <summary>
        /// Convert the vertices to fit into the [-1, -1]^3 display cube
        /// </summary>
        /// <param name="vertices">vertices to convert</param>
        public static void Normalize(Vector3[] vertices)
        {
            Matrix4 transform = GetNormalizingMatrix(vertices);
            for(int i = 0; i < vertices.Length; ++i)
            {
                vertices[i] = new Vector3( new Vector4(vertices[i], 1.0f) * transform);
            }
        }

        /// <summary>
        /// Compute a matrix to convert vertices to fit into [-1, -1]^3 display device cube.
        /// </summary>
        /// <param name="vertices">list of vertices to to compute the matrix for</param>
        /// <returns>matrix transformation that will convert the vertices to the opengl normal form</returns>
        public static Matrix4 GetNormalizingMatrix(Vector3[] vertices)
        {
            // Find the min and max value on each axis
            Vector3 min = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
            Vector3 max = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

            // compute the size of the AABB and it's center
            foreach (var v in vertices)
            {
                max.X = Math.Max(max.X, v.X);
                max.Y = Math.Max(max.Y, v.Y);
                max.Z = Math.Max(max.Z, v.Z);
                min.X = Math.Min(min.X, v.X);
                min.Y = Math.Min(min.Y, v.Y);
                min.Z = Math.Min(min.Z, v.Z);
            }
            var size = max - min;
            var center = (min + max) * 0.5f;

            float rx = 2.0f / size.X;
            float ry = 2.0f / size.Y;
            float rz = 2.0f / size.Z;

            // compute the scale factor needed
            float factor = Math.Min(rx, Math.Min(ry, rz));

            // compute the translation and factor matrices
            return Matrix4.CreateTranslation(new Vector3() - center) * Matrix4.CreateScale(factor);
        }
    }
}

