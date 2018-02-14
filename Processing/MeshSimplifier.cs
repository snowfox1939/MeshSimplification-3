using System;
using OpenTK;
using Polynano.Common;
using Polynano.Helpers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Polynano.Processing
{
    internal class MeshSimplifier : IMeshSimplifier
    {
        // We use dictionaries to preserve the original indices 
        // because updating them is too expensive.

        private Vertex[] _vertices;

        private bool[] _vertexRemoved;

        private Dictionary<int, IndexedTriangle> _faces;

        private FastEdgeContrainer _edges;

        private Stack<ContractionHistory> _history;

        public IReadOnlyList<Vertex> Vertices => _vertices;

        public IReadOnlyDictionary<int, IndexedTriangle> Faces => _faces;

        public IEnumerable<IndexedEdge> Edges => _edges.Edges;

        public MeshGeometryData GetModel()
        {
            var currentModel = new MeshGeometryData
            {
                Vertices = Vertices.Select(v => v.Position).ToArray(),
                Faces = Faces.OrderBy(f => f.Key).Select(face => face.Value).ToArray(),
                Normals = Vertices.Select(n => n.Normal).ToArray(),
                ActiveEdges = Edges.ToArray(),
                ActiveVertices = new int[_vertexRemoved.Count(c => c == false)]
            };

            int k = 0;
            int i = 0;
            foreach(var remove in _vertexRemoved)
            {
                if(!remove)
                {
                    currentModel.ActiveVertices[k] = i;
                     k++;
                }
                i++;
            }
            return currentModel;
        }

        public void SetStructure(MeshData data)
        {
            _vertices = new Vertex[data.Vertices.Length];
            _vertexRemoved = new bool[data.Vertices.Length];
            _faces = new Dictionary<int, IndexedTriangle>(data.Faces.Length);
            _edges = new FastEdgeContrainer(data.Vertices.Length);
            _history = new Stack<ContractionHistory>();

            for (int i = 0; i < data.Vertices.Length; i++)
            {
                _vertices[i] = new Vertex(data.Vertices[i], data.Normals?[i] ?? Vector3.Zero);
            }

            for (int i = 0; i < data.Faces.Length; i++)
            {
                _faces.Add(i, data.Faces[i]);
            }

            foreach (var face in _faces)
            {
                IndexedTriangle f = face.Value;

                int v1 = f.Vertex1;
                int v2 = f.Vertex2;
                int v3 = f.Vertex3;

                var e1 = f.Edge1;
                var e2 = f.Edge2;
                var e3 = f.Edge3;

                _edges.Add(e1);
                _edges.Add(e2);
                _edges.Add(e3);

                _vertices[v1].ConnectedFaces.Add(face.Key);
                _vertices[v2].ConnectedFaces.Add(face.Key);
                _vertices[v3].ConnectedFaces.Add(face.Key);
            }

            if (data.Normals == null)
            {
                for (int i = 0; i < _vertices.Length; i++)
                {
                    _vertices[i].Normal = GetNormal(i);
                }
            }

            int j = 0;
            foreach (var vertex in _vertices)
            {
                if (vertex.ConnectedFaces.Count == 0)
                    _vertexRemoved[j] = true;
                j++;
            }

            foreach (var edge in _edges.Edges)
            {
                UpdateContractionCost(edge);
            }
        }

        private void UpdateContractionCost(IndexedEdge edge)
        {
             var v1 = _vertices[edge.Vertex1];
             var v2 = _vertices[edge.Vertex2];

             var opt = (v1.Position + v2.Position) / 2.0f;

            var candidate = new ContractionCandidate(edge);

            var eo = _edges.TryGetEdge(edge);

             float cost = 0;
             foreach (int face in v1.ConnectedFaces.Concat(v2.ConnectedFaces))
             {
                 var f = _faces[face];
                 cost += GeometryMath.GetPointPlaneDistance(opt, _vertices[f.Vertex1].Position, _vertices[f.Vertex2].Position, _vertices[f.Vertex3].Position);
             }

            candidate.Cost =  v1.Error + v2.Error + cost;
            candidate.OptimalPosition = opt;

            if (eo == null || eo.OptimalPosition != opt || eo.Cost != candidate.Cost)
            {
                _edges.UpdateCandidate(candidate);
            }
        }

        public ContractionHistory SimplifyOneStep()
        {
            var c = _edges.GetBestCandidate();

            if (c != null)
            {
                return ContractEdge(_edges.GetBestCandidate());
            }

            return null;
        }

        private ContractionHistory ContractEdge(ContractionCandidate candidate)
        {
            var a = candidate.Edge.Vertex1;
            var b = candidate.Edge.Vertex2;
            var v1 = _vertices[a];
            var v2 = _vertices[b];

            var history = new ContractionHistory
            {
                Edge = candidate.Edge,
                OptimalPosition = candidate.OptimalPosition
            };

            foreach (var face in v1.ConnectedFaces.Intersect(v2.ConnectedFaces).ToList())
            {
                var f = _faces[face];

                history.NoteModifiedVertex(f.Vertex1, _vertices[f.Vertex1]);
                history.NoteModifiedVertex(f.Vertex2, _vertices[f.Vertex2]);
                history.NoteModifiedVertex(f.Vertex3, _vertices[f.Vertex3]);
                history.NoteModifiedFace(face, f);

                _vertices[f.Vertex1].ConnectedFaces.Remove(face);
                _vertices[f.Vertex2].ConnectedFaces.Remove(face);
                _vertices[f.Vertex3].ConnectedFaces.Remove(face);

                _vertexRemoved[f.Vertex1] = _vertices[f.Vertex1].ConnectedFaces.Count == 0;
                _vertexRemoved[f.Vertex2] = _vertices[f.Vertex2].ConnectedFaces.Count == 0;
                _vertexRemoved[f.Vertex3] = _vertices[f.Vertex3].ConnectedFaces.Count == 0;

                _edges.Remove(f.Edge1);
                _edges.Remove(f.Edge2);
                _edges.Remove(f.Edge3);

                _faces.Remove(face);
            }

            foreach (var face in v1.ConnectedFaces.Concat(v2.ConnectedFaces).Distinct().ToArray())
            {
                var f = _faces[face];
                history.NoteModifiedFace(face, f);

                if (f.HasVertex(b))
                {

                    history.NoteModifiedVertex(a, _vertices[a]);
                    history.NoteModifiedVertex(b, _vertices[b]);

                    _edges.Remove(f.Edge1);
                    _edges.Remove(f.Edge2);
                    _edges.Remove(f.Edge3);

                    f.ReplaceVertex(b, a);
                    _faces[face] = f;

                    _vertices[a].ConnectedFaces.Add(face);
                    _vertices[b].ConnectedFaces.Remove(face);
                }
                _edges.Add(f.Edge1);
                _edges.Add(f.Edge2);
                _edges.Add(f.Edge3);
            }

            Debug.Assert(v2.ConnectedFaces.Count == 0);
            _vertexRemoved[b] = true;
            _vertexRemoved[a] = _vertices[a].ConnectedFaces.Count == 0;

            history.NoteModifiedVertex(a, _vertices[a]);
            _vertices[a].Position = candidate.OptimalPosition;
            _vertices[a].Error = candidate.Cost;

            foreach (var edge in GetVertexEdges(a))
            {
                int nvi = edge.Vertex1 == a ? edge.Vertex2 : edge.Vertex1;
                foreach (var nv in GetVertexEdges(nvi))
                {
                    UpdateContractionCost(nv);
                }
            }
            _history.Push(history);
            return history;
        }

        public void RevertOneStep()
        {
            if (_history.Count == 0)
            {
                return;
            }

            var history = _history.Pop();

            foreach (var v in history.Vertices)
            {
                _vertices[v.Key] = v.Value;
            }

            foreach (var f in history.Faces)
            {
                _edges.Remove(f.Value.Edge1);
                _edges.Remove(f.Value.Edge2);
                _edges.Remove(f.Value.Edge3);

                if (_faces.ContainsKey(f.Key))
                {
                    var face = _faces[f.Key];
                    _edges.Remove(face.Edge1);
                    _edges.Remove(face.Edge2);
                    _edges.Remove(face.Edge3);
                }
            }

            foreach(var f in history.Faces)
            {
                _faces[f.Key] = f.Value;
                _edges.Add(f.Value.Edge1);
                _edges.Add(f.Value.Edge2);
                _edges.Add(f.Value.Edge3);
                _vertexRemoved[f.Value.Vertex1] = false;
                _vertexRemoved[f.Value.Vertex2] = false;
                _vertexRemoved[f.Value.Vertex3] = false;
            }

            foreach (var edge in GetVertexEdges(history.Edge.Vertex1))
            {
                int nvi = edge.Vertex1 == history.Edge.Vertex2 ? edge.Vertex2 : edge.Vertex1;
                foreach (var nv in GetVertexEdges(nvi))
                {
                    UpdateContractionCost(nv);
                }
            }
        }


        private IEnumerable<IndexedEdge> GetVertexEdges(int vertexIndex)
        {
            // @TODO optimize
            HashSet<int> dup = new HashSet<int>();
            foreach (int face in _vertices[vertexIndex].ConnectedFaces)
            {
                var f = _faces[face];
                if (f.Edge1.ContainsVertex(vertexIndex))
                {
                    dup.Add(f.Edge1.Vertex1 == vertexIndex ? f.Edge1.Vertex2 : f.Edge1.Vertex1);
                }
                if (f.Edge2.ContainsVertex(vertexIndex))
                {
                    dup.Add(f.Edge2.Vertex1 == vertexIndex ? f.Edge2.Vertex2 : f.Edge2.Vertex1);
                }
                if (f.Edge3.ContainsVertex(vertexIndex))
                {
                    dup.Add(f.Edge3.Vertex1 == vertexIndex ? f.Edge3.Vertex2 : f.Edge3.Vertex1);
                }
            }

            foreach (int d in dup)
            {
                yield return new IndexedEdge(vertexIndex, d);
            }
        }

        private Vector3 GetNormal(int vertexIndex)
        {
            Vector3 v = Vector3.Zero;
            int c = 0;
            foreach (int f in Vertices[vertexIndex].ConnectedFaces)
            {
                var face = Faces[f];

                var v1 = Vertices[face.Vertex1];
                var v2 = Vertices[face.Vertex2];
                var v3 = Vertices[face.Vertex3];

                v += GeometryMath.GetTriangleNormal(v1.Position, v2.Position, v3.Position);
                c++;
            }

            return v / c;
        }
    }
}