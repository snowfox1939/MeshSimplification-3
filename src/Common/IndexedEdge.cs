using System;

namespace Polynano.Common
{
    /// <summary>
    /// PDO for a Edge. 
    /// </summary>
    public struct IndexedEdge
    {
        public int Vertex1;

        public int Vertex2;

        /// <summary>
        /// Default constructor,
        /// note that the smaller index is ALWAYS stored as Vertex1
        /// and the bigger as Vertex2
        /// this allows to get rid of the duplicates e.g. 
        /// 1;2 is the same as 2;1. 
        /// </summary>
        /// <param name="v1">index of the first vertex</param>
        /// <param name="v2">index of the second vertex</param>
        public IndexedEdge(int v1, int v2)
        {
            if (v1 == v2)
            {
                throw new ArgumentException();
            }

            Vertex1 = Math.Min(v1, v2);
            Vertex2 = Math.Max(v1, v2);
        }

        /// <summary>
        /// Whether this edge has the vertex v
        /// </summary>
        /// <param name="v">vertex index to check</param>
        /// <returns>true if this edge contains the vertex, false otherwise</returns>
        public bool ContainsVertex(int v)
        {
            return Vertex1 == v || Vertex2 == v;
        }

        /// <summary>
        /// Get the size of this struct in bytes.
        /// needed for openGL.
        /// </summary>
        /// <returns>size in bytes of the struct</returns>
        public static int SizeInBytes => sizeof(int) * 2;
    }
}