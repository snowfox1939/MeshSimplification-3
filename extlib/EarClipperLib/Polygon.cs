using System.Collections.Generic;
using System.Diagnostics;

namespace EarClipperLib
{
    internal class Polygon
    {
        internal ConnectionEdge Start;
        internal int PointCount = 0;

        internal IEnumerable<ConnectionEdge> GetPolygonCirculator()
        {
            if (Start == null) { yield break; }
            var h = Start;
            do
            {
                yield return h;
                h = h.Next;
            }
            while (h != Start);
        }

        internal void Remove(ConnectionEdge cur)
        {
            cur.Prev.Next = cur.Next;
            cur.Next.Prev = cur.Prev;
            var incidentEdges = (List<ConnectionEdge>)cur.Origin.DynamicProperties.GetValue(PropertyConstants.IncidentEdges);
            int index = incidentEdges.FindIndex(x => x.Equals(cur));
            Debug.Assert(index >= 0);
            incidentEdges.RemoveAt(index);
            if (incidentEdges.Count == 0)
                PointCount--;
            if (cur == Start)
                Start = cur.Prev;
        }

        public bool Contains(Vector3m vector2M, out Vector3m res)
        {
            foreach (var connectionEdge in GetPolygonCirculator())
            {
                if (connectionEdge.Origin.Equals(vector2M))
                {
                    res = connectionEdge.Origin;
                    return true;
                }
            }
            res = null;
            return false;
        }
    }
}