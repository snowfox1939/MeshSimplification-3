using System;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace Polynano.Graphics.Helpers
{
    /// <summary>
    // Wrapper around the OpenGL Vertex Array
    // that owns the array and therefore is responsible
    // for deleting it.
    /// </summary>
    public class VertexArray : IDisposable
    {
        private int _handle;

        public VertexArray()
        {
            // create the array
            _handle = GL.GenVertexArray();
        }

        ~VertexArray()
        {
            Dispose();
        }

        /// <summary>
        /// Get the GL Vertex Array handle
        /// </summary>
        public int Handle
        {
            get
            {
                return _handle;
            }
        }

        /// <summary>
        /// Bind buffers to this VertexArray
        /// </summary>
        /// <param name="vertexBuffer">vertex buffer to bind</param>
        /// <param name="normalBuffer">normal buffer to bind</param>
        /// <param name="indexBuffer">index buffer to bind</param>
        public void BindBuffers(DataBuffer vertexBuffer, DataBuffer normalBuffer, DataBuffer indexBuffer)
        {
            // bind the vertex array
            GL.BindVertexArray(_handle);

            // enable the first attribute
            GL.EnableVertexAttribArray(0);

            // bind the vertex buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer.Handle);
            // define the attribute pointer
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, Vector3.SizeInBytes, 0);

            // bind the index buffer
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer.Handle);

            if (normalBuffer != null)
            {
                // enable second attribute
                GL.EnableVertexAttribArray(1);
                // bind the normal buffer
                GL.BindBuffer(BufferTarget.ArrayBuffer, normalBuffer.Handle);
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, true, Vector3.SizeInBytes, 0);
            }

            // unbind the array
            GL.BindVertexArray(0);
        }

        /// <summary>
        /// Dispose the array
        /// </summary>
        public void Dispose()
        {
            if (_handle != 0)
            {
                GL.DeleteVertexArray(_handle);
                _handle = 0;
            }
        }
    }
}
