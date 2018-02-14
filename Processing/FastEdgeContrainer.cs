using Polynano.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Polynano.Processing
{
    internal class FastEdgeContrainer
    {
        private readonly List<ContractionCandidate>[] _edges;

        private readonly SortedDictionary<float, List<IndexedEdge>> _index;

        public FastEdgeContrainer(int vertexCount)
        {
            _edges = new List<ContractionCandidate>[vertexCount];
            _index = new SortedDictionary<float, List<IndexedEdge>>();

            for (int i = 0; i < vertexCount; i++)
            {
                _edges[i] = new List<ContractionCandidate>();
            }
        }

        public IEnumerable<IndexedEdge> Edges
        {
            get
            {
                for (int i = 0; i < _edges.Length; i++)
                {
                    foreach (var e in _edges[i])
                    {
                        yield return e.Edge;
                    }
                }
            }
        }

        public void Add(IndexedEdge edge)
        {
            if (_edges[edge.Vertex1].Count(c => c.Edge.Vertex2 == edge.Vertex2) == 0)
            {
                _edges[edge.Vertex1].Add(new ContractionCandidate(edge));
            }
        }

        public void Remove(IndexedEdge edge)
        {
            var e = TryGetEdge(edge);

            if (e == null)
            {
                return;
            }

            RemoveFromIndex(e);
            _edges[edge.Vertex1].RemoveAll(c => c.Edge.Vertex2 == edge.Vertex2);
        }

        public bool Exists(IndexedEdge edge)
        {
            return _edges[edge.Vertex1].Count(e => e.Edge.Vertex1 == edge.Vertex1 && e.Edge.Vertex2 == edge.Vertex2) > 0;
        }


        public void UpdateCandidate(ContractionCandidate candidate)
        {
            var edge = GetEdge(candidate.Edge);

            RemoveFromIndex(edge);

            edge.Cost = candidate.Cost;
            edge.OptimalPosition = candidate.OptimalPosition;


            if (_index.TryGetValue(edge.Cost, out var edges))
            {
                edges.Add(edge.Edge);
            }
            else
            {
                _index.Add(edge.Cost, new List<IndexedEdge> { edge.Edge });
            }
        }

        public ContractionCandidate GetBestCandidate()
        {
            if (_index.Count == 0)
            {
                return null;
            }
            return GetEdge(_index.First().Value.First());
        }

        public ContractionCandidate TryGetEdge(IndexedEdge edge)
        {
            for (int i = 0; i < _edges[edge.Vertex1].Count; i++)
            {
                if (_edges[edge.Vertex1][i].Edge.Vertex2 == edge.Vertex2)
                {
                    return _edges[edge.Vertex1][i];
                }
            }

            return null;
        }

        public ContractionCandidate GetEdge(IndexedEdge edge)
        {
            var e = TryGetEdge(edge);
            if (e == null)
            {
                throw new ArgumentException();
            }
            return e;
        }

        private void RemoveFromIndex(ContractionCandidate candidate)
        {
            int v1 = candidate.Edge.Vertex1;
            int v2 = candidate.Edge.Vertex2;

            if (_index.TryGetValue(candidate.Cost, out var edges))
            {
                for (int i = 0; i < edges.Count; i++)
                {
                    if (edges[i].Vertex1 == v1 && edges[i].Vertex2 == v2)
                    {
                        var temp = edges[edges.Count - 1];
                        edges[edges.Count - 1] = edges[i];
                        edges[i] = temp;
                        edges.RemoveAt(edges.Count - 1);
                        break;
                    }
                }
                if (edges.Count == 0)
                {
                    _index.Remove(candidate.Cost);
                }
            }
        }
    }
}