using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using Polynano.Graphics.Helpers;
using System.Drawing;

namespace Polynano.Graphics
{
    /// <summary>
    /// Class Responsible for the Display of a Mesh.
    /// </summary>
    public class MeshView : GLControl
    {
        /// <summary>
        /// Meshes to draw
        /// </summary>
        private IEnumerable<IMesh> _meshes;

        /// <summary>
        /// Color mapping to draw the meshes
        /// the length should always equal to the count of the meshes
        /// </summary>
        private IEnumerable<Vector3> _colorMappings;

        /// <summary>
        /// Camera, responsible for the movement and zoom.
        /// </summary>
        private ArcballCamera _camera;

        /// <summary>
        /// Whether the rotation is active e.g.
        /// when the user moves the mouse a rotation should be performed.
        /// </summary>
        private bool _isViewRotationActive;

        /// <summary>
        /// The position of the mouse, retrieved from the last event.
        /// </summary>
        private Point _lastMousePos;

        /// <summary>
        /// Will be fired when the MeshView is ready.
        /// </summary>
        public event EventHandler OnReady;

        /// <summary>
        /// Default color to draw the meshes
        /// </summary>
        private static readonly Vector3 DefaultMeshColor = new Vector3(0.6f, 0, 0);

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MeshView()
            : base(new GraphicsMode(32, 24, 0, 8), 3, 3, GraphicsContextFlags.Default)
        {
            // set the OpenGL to 3.3
            // intitialize the Camera with the current width and height
            _camera = new ArcballCamera(Width, Height);
            // set the default mouse button to the left button
            ViewNavigationTriggerButton = MouseButtons.Left;
            _isViewRotationActive = false;
        }

        /// <summary>
        /// get or set the mouse button that will start the mouse navigation
        /// </summary>
        public MouseButtons ViewNavigationTriggerButton { get; set; }

        /// <summary>
        /// Get or set the ShaderProgram that will be used for rendering
        /// </summary>
        /// <returns>ShaderProgram used</returns>
        public IShaderProgram ShaderProgram { get; set; }

        /// <summary>
        /// Get or set the transformation that will be performed on the mesh
        /// </summary>
        /// <returns>current transformation</returns>
        public Matrix4 Transformation
        {
            get => _camera.ModelMatrix;
            set => _camera = new ArcballCamera(Width, Height, value);
        }

        /// <summary>
        /// Dispose the object
        /// </summary>
        public new void Dispose()
        {
            ShaderProgram?.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Get or set the Mesh to draw.
        /// it's called meshes as one mesh may be build from smaller meshes
        /// </summary>
        /// <returns>Mesh that is currently renderered</returns>
        public IEnumerable<IMesh> Meshes
        {
            set
            {
                // make sure there are no NULLS.
                foreach (IMesh mesh in value)
                {
                    if (mesh == null)
                        throw new ArgumentNullException(nameof(Meshes));
                }

                _meshes = value;
            }
        }

        /// <summary>
        /// Get or set the colors to use to draw the parts of th mesh
        /// </summary>
        public IEnumerable<Vector3> MeshColorMappings
        {
            set => _colorMappings = value;
        }

        /// <summary>
        /// Called by default by the Windows when the OpenTK is ready
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            ShaderProgram = new ShaderProgram();
            OnReady?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called by default by Windows when the window has been resized
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            if (_camera != null)
            {
                _camera.ClientWidth = Width;
                _camera.ClientHeight = Height;
            }
        }

        /// <summary>
        /// Called by windows when the used used the mouse scroll
        /// </summary>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            _camera.Zoom(e.Delta > 0 ? 0.1f * e.Delta + 1.0f : 0.1f * e.Delta - 1.0f);
            Render();
        }

        /// <summary>
        /// Called by Windows when the element needs to be redrawn.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            Render();
        }

        /// <summary>
        /// Called by Windows when the user pressed a mouse button
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == ViewNavigationTriggerButton)
            {
                _isViewRotationActive = true;
                _lastMousePos = new Point(e.X, e.Y);
            }
        }

        /// <summary>
        /// Called by Windows when the user released a mouse button
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == ViewNavigationTriggerButton)
            {
                _isViewRotationActive = false;
            }
        }

        /// <summary>
        /// Called by Windows when the user moved the mouse
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_lastMousePos.X == e.X && _lastMousePos.Y == e.Y)
                return;

            // run only when the move was while the navigation button was pressed
            if (_isViewRotationActive)
            {
                _camera.Rotate(e.X, e.Y, _lastMousePos.X, _lastMousePos.Y);
                _lastMousePos = new Point(e.X, e.Y);
                Render();
            }
        }

        /// <summary>
        /// Render the Model to the screen
        /// </summary>
        private void Render()
        {
            if (ShaderProgram == null || ShaderProgram.ProgramId == 0)
                return;

            GL.ClearColor(Color.Black);

            if (_meshes != null)
            {
                GL.Enable(EnableCap.DepthTest);
                GL.UseProgram(ShaderProgram.ProgramId);

                ShaderProgram.ModelMatrix = _camera.ModelMatrix;
                ShaderProgram.ViewMatrix = _camera.ViewMatrix;
                ShaderProgram.ProjectionMatrix = _camera.ProjectionMatrix;

                GL.Viewport(0, 0, ClientSize.Width, ClientSize.Height);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                var c = _colorMappings ?? Enumerable.Repeat(DefaultMeshColor, _meshes.Count());
                var e = c.GetEnumerator();
                e.MoveNext();
                var color = e.Current;

                foreach (IMesh mesh in _meshes)
                {
                    GL.BindVertexArray(mesh.VaoHandle);

                    ShaderProgram.MeshColor = color;

                    if (e.MoveNext())
                    {
                        color = e.Current;
                    }

                    GL.DrawElements(mesh.Primitive, mesh.ElementCount,
                           DrawElementsType.UnsignedInt, IntPtr.Zero);
                }
                e.Dispose();
            }
            
            // Render to screen
            SwapBuffers();
        }
    }
}
