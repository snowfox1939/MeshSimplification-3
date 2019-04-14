namespace Polynano.Common
{
    /// <summary>
    /// Represents a 3D Model
    /// Also provides information about the edges 
    /// and used vertices
    /// </summary>
    public class MeshGeometryData : MeshData
    {
        public int[] ActiveVertices;

        public IndexedEdge[] ActiveEdges;
    }
}
