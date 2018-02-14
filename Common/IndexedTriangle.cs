using System;

namespace Polynano.Common
{
    /// <summary>
    /// PDO for a Triangle
    /// </summary>
    public struct IndexedTriangle
    {
        public int Vertex1;

        public int Vertex2;

        public int Vertex3;

        public IndexedTriangle(int v1, int v2, int v3)
        {
            if(v1 == v2 || v1 == v3 || v2 == v3)
            {
                throw new ArgumentException();
            }

            Vertex1 = v1;
            Vertex2 = v2;
            Vertex3 = v3;
        }

        /// <summary>
        /// Whether the Triangle is made of vertex v.
        /// </summary>
        /// <param name="v">index of the vertex to check</param>
        /// <returns>true if this triangle is made of v</returns>
        public bool HasVertex(int v)
        {
            return Vertex1 == v || Vertex2 == v || Vertex3 == v;
        }

        /// <summary>
        /// Replace the vertex v in the triangle to nv.
        /// </summary>
        /// <param name="v">index of the vertex to replace</param>
        /// <param name="nv">index to replace the vertex with.</param>
        public void ReplaceVertex(int v, int nv)
        {
            if(nv == Vertex1 || nv == Vertex2 || nv == Vertex3)
            {
                throw new ArgumentException();
            }

            if(Vertex1 == v)
                Vertex1 = nv;
            else if(Vertex2 == v)
                Vertex2 = nv;
            else if(Vertex3 == v)
                Vertex3 = nv;
            else
                throw new ArgumentException();
        }

        /// <summary>
        /// Get the first edge of the triangle
        /// </summary>
        /// <returns>First edge as PDO</returns>
        public IndexedEdge Edge1 => new IndexedEdge(Vertex1, Vertex2);

        /// <summary>
        /// Get the second edge of the triangle
        /// </summary>
        /// <returns>Edge as PDO</returns>
        public IndexedEdge Edge2 => new IndexedEdge(Vertex2, Vertex3);

        /// <summary>
        /// Get the third edge of the triangle
        /// </summary>
        /// <returns>Edge as PDO</returns>
        public IndexedEdge Edge3 => new IndexedEdge(Vertex3, Vertex1);

        /// <summary>
        /// Get the size in bytes of this struct.
        /// Needed for OpenGL.
        /// </summary>
        /// <returns>size of the struct in bytes</returns>
        public static int SizeInBytes => sizeof(int) * 3;
    }
}