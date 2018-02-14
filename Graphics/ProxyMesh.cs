using System;
using OpenTK.Graphics.OpenGL;
using Polynano.Graphics.Helpers;

namespace Polynano.Graphics
{
    /// <summary>
    // A ProxyMesh is a Mesh that reuses the normal and vertex buffers
    // of an already existing Mesh.
    // If the original Mesh gets disposed the proxy 
    // will be in a invalid state.
    /// </summary>
    internal sealed class ProxyMesh : IMesh, IDisposable
    {
        private VertexArray _vaoHandle;

        private readonly DataBuffer _indexBuffer;

        public ProxyMesh()
        {
            _indexBuffer = new DataBuffer();
        }

        ~ProxyMesh()
        {
            Dispose();
        }

        /// <summary>
        /// should be called only by Mesh, refer to Mesh for more information
        /// </summary>
        public void SetMesh<T>(DataBuffer vertices, DataBuffer normals, T[] indices, PrimitiveType primitive) where T : struct
        {
            Primitive = primitive;
            ElementCount = PrimitiveHelper.GetElementCountForPrimitive(primitive) * indices.Length;

            _vaoHandle?.Dispose();
            _vaoHandle = new VertexArray();

            if (indices.Length == 0)
            {
                ElementCount = 0;
                return;
            }

            // ignore the normals on lines and points
            _vaoHandle.BindBuffers(vertices, PrimitiveHelper.GetElementCountForPrimitive(primitive) <= 2 ? null : normals, _indexBuffer);
            _indexBuffer.BufferData(BufferTarget.ElementArrayBuffer, indices, ElementCount * sizeof(int));
        }

        /// <summary>
        /// Update the mesh with new indices and primitive
        /// </summary>
        public void UpdateMesh<T>(T[] indices, PrimitiveType primitive) where T : struct
        {
            GL.BindVertexArray(0); 
            Primitive = primitive;
            ElementCount = indices.Length * PrimitiveHelper.GetElementCountForPrimitive(primitive);
            _indexBuffer.BufferSubData(BufferTarget.ElementArrayBuffer, indices, ElementCount * sizeof(int), IntPtr.Zero);
        }

        public void Dispose()
        {
            _vaoHandle.Dispose();
            _indexBuffer?.Dispose();
            ElementCount = 0;
        }

        /// <summary>
        /// Get the index of the Gl Vertex Array
        /// </summary>
        /// <returns></returns>
        public int VaoHandle => _vaoHandle.Handle;

        /// <summary>
        /// Get the count of the primitives
        /// </summary>
        public int ElementCount { get; private set; }

        /// <summary>
        /// Get the mesh primitive
        /// </summary>
        public PrimitiveType Primitive { get; private set; }
    }
}
