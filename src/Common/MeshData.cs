using OpenTK;

namespace Polynano.Common
{
    /// <summary>
    /// Represens a 3D Model.
    /// </summary>
    public class MeshData
    {
        public Vector3[] Vertices;

        public Vector3[] Normals;

        public IndexedTriangle[] Faces;
    }
}