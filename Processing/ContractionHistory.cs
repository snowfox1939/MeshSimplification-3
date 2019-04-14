using System;
using Polynano.Common;
using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace Polynano.Processing
{
    public class ContractionHistory
    {
        /// <summary>
        /// Affected vertices
        /// </summary>
        public Dictionary<int, Vertex> Vertices;

        /// <summary>
        /// Affected Triangles
        /// </summary>
        public Dictionary<int, IndexedTriangle> Faces;

        /// <summary>
        /// The Edge that was contracted
        /// </summary>
        public IndexedEdge Edge;

        public Vector3 OptimalPosition;

        public ContractionHistory()
        {
            Vertices = new Dictionary<int, Vertex>();
            Faces = new Dictionary<int, IndexedTriangle>();
        }

        public void NoteModifiedVertex(int index, Vertex original)
        {
            if (!Vertices.ContainsKey(index))
            {
                var o = new Vertex(original.Position, original.Normal)
                {
                    ConnectedFaces = original.ConnectedFaces.ToList()
                };

                Vertices.Add(index, o);
            }
        }

        public void NoteModifiedFace(int index, IndexedTriangle original)
        {
            if(!Faces.ContainsKey(index))
            {
                Faces.Add(index, original);
            }
        }
    }
}
