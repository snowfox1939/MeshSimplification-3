using OpenTK;
using Polynano.Common;

namespace Polynano.Processing
{
    internal class ContractionCandidate
    {
        public IndexedEdge Edge;

        public float Cost;

        public Vector3 OptimalPosition;

        public ContractionCandidate(IndexedEdge e)
        {
            Edge = e;
            Cost = 0;
            OptimalPosition = Vector3.Zero;
        }
    }
}
