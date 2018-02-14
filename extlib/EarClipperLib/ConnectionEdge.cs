using System.Collections.Generic;

namespace EarClipperLib
{
    internal class ConnectionEdge
    {
        protected bool Equals(ConnectionEdge other)
        {
            return Next.Origin.Equals(other.Next.Origin) && Origin.Equals(other.Origin);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ConnectionEdge)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Next.Origin != null ? Next.Origin.GetHashCode() : 0) * 397) ^ (Origin != null ? Origin.GetHashCode() : 0);
            }
        }

        internal Vector3m Origin { get; private set; }
        internal ConnectionEdge Prev;
        internal ConnectionEdge Next;
        internal Polygon Polygon { get; set; }

        public ConnectionEdge(Vector3m p0, Polygon parentPolygon)
        {
            Origin = p0;
            Polygon = parentPolygon;
            AddIncidentEdge(this);
        }

        public override string ToString()
        {
            return "Origin: " + Origin + " Next: " + Next.Origin;
        }

        internal void AddIncidentEdge(ConnectionEdge next)
        {
            var list = (List<ConnectionEdge>)Origin.DynamicProperties.GetValue(PropertyConstants.IncidentEdges);
            list.Add(next);
        }


    }
}