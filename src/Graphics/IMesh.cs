using OpenTK.Graphics.OpenGL;

namespace Polynano.Graphics
{
    /// <summary>
    /// Interface for drawable Mesh.
    /// </summary>
    public interface IMesh
    {
        /// <summary>
        /// Id of the OpenGL Vertex Array that stores the mesh.
        /// </summary>
        /// <returns></returns>
        int VaoHandle { get; }

        /// <summary>
        /// Count of the individual elements that will be drawn
        /// eg. number of vertices
        /// </summary>
        int ElementCount { get; }

        /// <summary>
        /// The Primitive of the Mesh.
        /// </summary>
        PrimitiveType Primitive { get; }
    }
}
