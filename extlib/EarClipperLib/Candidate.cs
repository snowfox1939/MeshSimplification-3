using Microsoft.SolverFoundation.Common;

namespace EarClipperLib
{
    internal class Candidate
    {
        internal Rational currentDistance = double.MaxValue;
        internal Vector3m I;
        internal ConnectionEdge Origin;
        internal int PolyIndex;
    }
}