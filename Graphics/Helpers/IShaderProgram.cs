using OpenTK;
using System;

namespace Polynano.Graphics.Helpers
{
    /// <summary>
    /// Interface for a GL shader program.
    /// </summary>
    public interface IShaderProgram : IDisposable
    {
        /// <summary>
        /// index of the gl ShaderProgram
        /// </summary>
        /// <returns></returns>
        int ProgramId { get; }

        /// <summary>
        /// method to set the model matrix.
        /// </summary>
        Matrix4 ModelMatrix { set; }

        /// <summary>
        /// method to set the view matrix 
        /// </summary>
        Matrix4 ViewMatrix { set; }

        /// <summary>
        /// method to set the projection matrix 
        /// </summary>
        Matrix4 ProjectionMatrix { set; }

        /// <summary>
        /// method to set the mesh color.
        /// </summary>
        Vector3 MeshColor { set; }
    }
}
