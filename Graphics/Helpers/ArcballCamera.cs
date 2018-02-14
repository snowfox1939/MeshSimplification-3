using System;
using OpenTK;

namespace Polynano.Graphics.Helpers
{
    /// <summary>
    /// Responsible for the camera.
    /// </summary>
    public class ArcballCamera
    {
        private static Matrix4 _lookAtMatrix = Matrix4.CreateTranslation(0, 0, -3);

        public ArcballCamera(int width, int height)
            : this(width, height, Matrix4.Identity)
        {
        }

        /// <summary>
        /// Construct the camera
        /// </summary>
        /// <param name="width">width of the display element</param>
        /// <param name="height">height of the display element</param>
        /// <param name="transformation">transformation to perform on the model</param>
        public ArcballCamera(int width, int height, Matrix4 transformation)
        {
            ClientWidth = width;
            ClientHeight = height;
            ViewMatrix = _lookAtMatrix;
            ModelMatrix = transformation;
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                (float)Math.PI / 4, ClientWidth / (float)(ClientHeight), 1, 100);
        }

        public Matrix4 ViewMatrix { get; private set; }

        public Matrix4 ProjectionMatrix { get; }

        public Matrix4 ModelMatrix { get; }

        public int ClientWidth { get; set; }

        public int ClientHeight { get; set; }

        /// <summary>
        /// Rotate the model
        /// </summary>
        /// <param name="lastx"></param>
        /// <param name="lasty"></param>
        /// <param name="currentx"></param>
        /// <param name="currenty"></param>
        public void Rotate(int lastx, int lasty, int currentx, int currenty)
        {
            var va = GetVectorToSphere(currentx, currenty);
            var vb = GetVectorToSphere(lastx, lasty);

            float angle = (float)Math.Acos(Math.Min(1.0f, Vector3.Dot(va, vb)));
            var axisInCameraCoord = Vector3.Cross(va, vb);

            ViewMatrix = ((ViewMatrix * _lookAtMatrix.Inverted()) * Matrix4.CreateFromAxisAngle(axisInCameraCoord, angle)) * _lookAtMatrix;
        }

        public void Zoom(float factor)
        {
            ViewMatrix = (ViewMatrix * _lookAtMatrix.Inverted()) * Matrix4.CreateScale(factor > 0 ? 1.1f : 0.9f) * _lookAtMatrix;
        }

        private Vector3 GetVectorToSphere(int x, int y)
        {
            Vector3 v = new Vector3
            {
                X = 1.0f * x / ClientWidth * 2f - 1.0f,
                Y = -(1.0f * y / ClientHeight * 2f - 1.0f),
                Z = 0f
            };

            float m = v.X * v.X + v.Y * v.Y;
            if (m <= 1 * 1)
            {
                v.Z = (float)Math.Sqrt(1 * 1 - m);
            }
            else
            {
                v.Normalize();
            }
            return v;
        }
    }
}
