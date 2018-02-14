using System;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;

namespace Polynano.Graphics.Helpers
{
    /// <summary>
    // Wrapper around the OpenGL Shader
    // that owns the shader and therefore is responsible
    // for deleting it.
    /// </summary>
    public class Shader : IDisposable
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="type">type of the shader</param>
        /// <param name="source">source code of the shader</param>
        public Shader(ShaderType type, string source)
        {
            // create and compile the GL Shader.

            Id = GL.CreateShader(type);

            GL.ShaderSource(Id, source);
            GL.CompileShader(Id);

            Debug.WriteLine(GL.GetShaderInfoLog(Id));
        }

        /// <summary>
        /// Get the index of the shader
        /// </summary>
        public int Id { get; private set; }

        ~Shader()
        {
            Dispose();
        }

        /// <summary>
        /// Dispose the buffer
        /// </summary>
        public void Dispose()
        {
            if (Id != 0)
            {
                GL.DeleteShader(Id);
                Id = 0;
            }
        }
    }
}
