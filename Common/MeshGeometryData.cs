namespace Polynano.Common
{
    /// <summary>
    /// Represents a 3D Model
    /// Also provides information about the edges 
    /// and used vertices
    /// </summary>
    internal class MeshGeometryData : MeshData
    {
        public int[] ActiveVertices;

        public IndexedEdge[] ActiveEdges;
    }
}
