using OpenTK;
using OpenTK.Graphics.OpenGL;
using Polynano.Common;
using Polynano.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Polynano.Processing;

namespace Polynano.UI
{
    /// <summary>
    /// Reponsible for the Model view control.
    /// </summary>
    internal class Viewer : IDisposable
    {
        /// <summary>
        /// The MeshView control
        /// </summary>
        private readonly MeshView _meshView;

        /// <summary>
        /// The face mesh to display
        /// </summary>
        private Mesh _faceMesh;

        /// <summary>
        /// The edge mesh to display
        /// </summary>
        private ProxyMesh _edgeMesh;

        /// <summary>
        /// The vertex mesh to display
        /// </summary>
        private ProxyMesh _vertexMesh;
        
        /// <summary>
        /// Array of the meshes to display
        /// </summary>
        private IMesh[] _meshes;

        /// <summary>
        /// Set the transformation to apply to the model
        /// </summary>
        public Matrix4 Transformation
        {
            set => _meshView.Transformation = value;
        }

        /// <summary>
        /// Whether to display faces
        /// </summary>
        public bool DisplayFaces
        {
            set
            {
                _meshes[0] = value ? _faceMesh : null;
                UpdateMeshView();
            }
        }

        /// <summary>
        /// Whether to display edges
        /// </summary>
        public bool DisplayEdges
        {
            set
            {
                _meshes[1] = value ? _edgeMesh : null;
                UpdateMeshView();
            }
        }

        /// <summary>
        /// whether to display vertices
        /// </summary>
        public bool DisplayVertices
        {
            set
            {
                _meshes[2] = value ? _vertexMesh : null;
                UpdateMeshView();
            }
        }

        public Viewer()
        {
            _meshes = null;
            _meshView = new MeshView();
        }


        public void Run()
        {
            _meshView.Run(OpenTK.DisplayDevice.Default.RefreshRate);
        }

        /// <summary>
        /// Update the size of this control
        /// </summary>
        public void UpdateSize(int x, int y, int width, int height)
        {
            _meshView.Width = width;
            _meshView.Height = height;
        }

        /// <summary>
        /// set the mesh
        /// </summary>
        /// <param name="data">data of the mesh to set</param>
        public void SetMesh(MeshGeometryData data)
        {
            _faceMesh?.Dispose();
            _faceMesh = new Mesh();

            _edgeMesh?.Dispose();
            _vertexMesh?.Dispose();

            _faceMesh.SetMesh(data.Vertices, data.Normals, data.Faces, PrimitiveType.Triangles);
            _edgeMesh = _faceMesh.GetProxyMesh(data.ActiveEdges, PrimitiveType.Lines);
            _vertexMesh = _faceMesh.GetProxyMesh(data.ActiveVertices, PrimitiveType.Points);

            if (_meshes == null)
            {
                _meshes = new IMesh[] { _faceMesh, null, null };
            }
            else
            {
                _meshes[0] = _meshes[0] != null ? _faceMesh : null;
                _meshes[1] = _meshes[1] != null ? _edgeMesh : null;
                _meshes[2] = _meshes[2] != null ? _vertexMesh : null;
            }

            UpdateMeshView();
        }

        /// <summary>
        /// Update the mesh.
        /// </summary>
        /// <param name="data">new data to update the mesh</param>
        public void UpdateMesh(MeshGeometryData data)
        {
            if (_faceMesh != null)
            {
                _faceMesh.UpdateMeshVertices(data.Vertices);
                _faceMesh.UpdateMesh(data.Faces, PrimitiveType.Triangles);
                _edgeMesh.UpdateMesh(data.ActiveEdges, PrimitiveType.Lines);
                _vertexMesh.UpdateMesh(data.ActiveVertices, PrimitiveType.Points);
                UpdateMeshView();
            }
        }
       
        public void Dispose()
        {
            _meshView?.Dispose();
            _faceMesh?.Dispose();
            _vertexMesh?.Dispose();
            _edgeMesh?.Dispose();
        }

        /// <summary>
        /// Update the colors and the mesh iterator to be passed to MeshView
        /// </summary>
        private void UpdateMeshView()
        {
            _meshView.Meshes = _meshes.Where(m => m != null);
            var colorMappings = new[]
            {
                new Vector3(0.6f, 0.0f, 0.0f),
                new Vector3(1.0f, 1.0f, 1.0f),
                new Vector3(0.0f, 1.0f, 0.0f),
            };
            var meshes = _meshes.ToArray();
            List<Vector3> colors = new List<Vector3>();
            for (int i = 0; i < meshes.Length; ++i)
            {
                if (meshes[i] == null)
                {
                }
                else
                {
                    colors.Add(colorMappings[i]);
                }
            }
            _meshView.MeshColorMappings = colors;
        }
    }
}
