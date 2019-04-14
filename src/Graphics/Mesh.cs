using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Polynano.Common;
using Polynano.Graphics.Helpers;

namespace Polynano.Graphics
{
    /// <summary>
    /// Represents a 3D Modell,
    /// manages the OpenGl buffers.
    /// </summary>
    class Mesh : IMesh, IDisposable
    {
        /// <summary>
        /// Vertex array of the mesh
        /// </summary>
        private VertexArray _vaoHandle;

        /// <summary>
        /// buffer to store the triangle/primitive indices 
        /// </summary>
        private readonly DataBuffer _indexBuffer;

        /// <summary>
        /// buffer to store the normal vectors
        /// </summary>
        private readonly DataBuffer _normalBuffer;

        /// <summary>
        /// buffer to store the vertex positions
        /// </summary>
        private readonly DataBuffer _vertexBuffer;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Mesh()
        {
            _vaoHandle = new VertexArray();
            _indexBuffer = new DataBuffer();
            _normalBuffer = new DataBuffer();
            _vertexBuffer = new DataBuffer();
        }

        ~Mesh()
        {
            Dispose();
        }

        /// <summary>
        /// Dispose the object and it`s buffers.
        /// </summary>
        public void Dispose()
        {
            _vaoHandle?.Dispose();
            ElementCount = 0;
            _vertexBuffer?.Dispose();
            _normalBuffer?.Dispose();
            _indexBuffer?.Dispose();
        }

        /// <summary>
        /// Get the index of the GL Vertex Array
        /// </summary>
        public int VaoHandle
        {
            get
            {
                return _vaoHandle.Handle;
            }
        }

        /// <summary>
        /// Get the primitive count
        /// </summary>
        public int ElementCount { get; private set; }

        /// <summary>
        /// Get the mesh primitive
        /// </summary>
        public PrimitiveType Primitive { get; private set; }

        /// <summary>
        /// Construct a proxyMesh, since the ProxyMesh is trusted
        /// we may pass our internal buffers to it.
        /// </summary>
        /// <param name="indices">the indices of the proxy mesh to create</param>
        /// <param name="primitive">the primitive type of the proxy mesh</param>
        /// <returns>Instance of the constructed ProxyMesh</returns>
        public ProxyMesh GetProxyMesh<T>(T[] indices, PrimitiveType primitive) where T : struct
        {
            var p = new ProxyMesh();
            p.SetMesh(_vertexBuffer, _normalBuffer, indices, primitive);
            return p;
        }

        /// <summary>
        /// Set a Mesh, send the vertices, normals and indices to OpenGl.
        /// </summary>
        /// <param name="vertices">list of vertex positions</param>
        /// <param name="normals">list of vertex normals</param>
        /// <param name="indices">list of primitive indices</param>
        /// <param name="primitive">the primitive type</param>
        public void SetMesh<T>(Vector3[] vertices, Vector3[] normals, T[] indices, PrimitiveType primitive) where T : struct
        {
            if (vertices == null || indices == null)
                throw new ArgumentNullException();

            if (vertices.Length == 0 || indices.Length == 0)
                throw new ArgumentException("Invalid vertices or faces count");

            // compute the number of vertices
            ElementCount = indices.Length * PrimitiveHelper.GetElementCountForPrimitive(primitive);
            Primitive = primitive;

            // send the vertices, normals and indices to the buffer.
            _vertexBuffer.BufferData(BufferTarget.ArrayBuffer, vertices, vertices.Length * Vector3.SizeInBytes);
            _indexBuffer.BufferData(BufferTarget.ElementArrayBuffer, indices, ElementCount * sizeof(int));
            if (normals != null && normals.Length != 0)
            {
                _normalBuffer.BufferData(BufferTarget.ArrayBuffer, normals, vertices.Length * Vector3.SizeInBytes);
            }

            // Remove the old Vertex array, if any, and construct a new one.
            if (_vaoHandle.Handle != 0)
            {
                _vaoHandle.Dispose();
                _vaoHandle = new VertexArray();
            }
            // bind the buffers to the VertexArray
            _vaoHandle.BindBuffers(_vertexBuffer, _normalBuffer, _indexBuffer);
        }

        /// <summary>
        /// Update the indices and the primitive of this mesh.
        /// Send the changes to OpenGL.
        /// </summary>
        /// <param name="indices">List of new indices</param>
        /// <param name="primitive">the new primitive</param>
        public void UpdateMesh<T>(T[] indices, PrimitiveType primitive) where T : struct
        {
            if (primitive != Primitive)
                throw new InvalidOperationException();

            // Unbind the previous vertex array.
            GL.BindVertexArray(0); // Important! 4 Hours wasted!
            Primitive = primitive;
            // update the buffer
            ElementCount = indices.Length * PrimitiveHelper.GetElementCountForPrimitive(primitive);
            _indexBuffer.BufferSubData(BufferTarget.ElementArrayBuffer, indices, ElementCount * sizeof(int), IntPtr.Zero);
        }

        /// <summary>
        /// Update the vertex positions of the emsh
        /// </summary>
        /// <param name="vertices">new list of position</param>
        public void UpdateMeshVertices(Vector3[] vertices)
        {
            // @TODO only update the vertices that changed
            _vertexBuffer.BufferSubData(BufferTarget.ArrayBuffer, vertices, vertices.Length * Vector3.SizeInBytes, IntPtr.Zero);
        }

        public void UpdateMeshVertex(int vertexIndex, Vector3 newVertex)
        {
            _vertexBuffer.BufferSubData(BufferTarget.ArrayBuffer, new Vector3[] { newVertex}, Vector3.SizeInBytes, new IntPtr(Vector3.SizeInBytes * vertexIndex));
        }

        public void UpdateMeshFace(int faceIndex, IndexedTriangle newFace)
        {
            _indexBuffer.BufferSubData(BufferTarget.ArrayBuffer, new [] { newFace }, IndexedTriangle.SizeInBytes, new IntPtr(IndexedTriangle.SizeInBytes * faceIndex));
        }
    }
}