using OpenTK;
using System.Collections.Generic;

namespace Polynano.Processing
{
    public class Vertex
    {
        public Vector3 Position;

        public Vector3 Normal;

        public float Error;

        public List<int> ConnectedFaces;

        public Vertex(Vector3 p)
        {
            Error = 0;
            Position = p;
            ConnectedFaces = new List<int>();
        }

        public Vertex(Vector3 p, Vector3 n)
            : this(p)
        {
            Normal = n;
        }
    }
}
