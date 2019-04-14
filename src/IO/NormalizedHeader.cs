using System;
using System.Collections.Generic;
using System.Linq;
using Polynano.Extensions;
using Polynano.IO.Ply;

namespace Polynano.IO
{
    /// <summary>
    /// Since the PLY Format is very flexible 
    /// we need some normalization to understand the header
    /// find the position of the vertices and faces from the header
    /// and store the indices in the members.
    /// </summary>
    internal class NormalizedHeader
    {
        public Header BaseHeader { get; }
        public int VerticesId  { get; }
        public int FacesId { get; }
        public int NormalsId { get; }
        public int VertexX { get; }
        public int VertexY { get; }
        public int VertexZ { get; }
        public int NormalX { get; }
        public int NormalY { get; }
        public int NormalZ { get; }
        public int VertexIndices { get; }

        /// <summary>
        /// Default constuctor
        /// </summary>
        /// <param name="header">Header to normalize</param>
        public NormalizedHeader(Header header)
        {
            BaseHeader = header;
            int FindElement(IEnumerable<string> aliases)
            {
                return header.Elements.FindIndex((e) => aliases.Contains(e.Name.ToLower()));
            }

            int FindProperty(int elemId, IEnumerable<string> aliases)
            {
                return header.Elements[elemId].Properties.FindIndex((p) => aliases.Contains(p.Name.ToLower()));
            }

            // Find the vertex, face and normal elements using some common names
            VerticesId = FindElement(new[] { "vertex", "vertices", "points" });
            FacesId = FindElement(new[] { "faces", "face", "triangles" });
            NormalsId = FindElement(new[] { "normals", "normal" });
            if(VerticesId == -1)
            {
                throw new ArgumentException("Model must define vertices");
            }

            if (FacesId == -1)
            {
                throw new ArgumentException("Model must define faces");
            }

            // find the x,y,z and index properties using the common names
            VertexX = FindProperty(VerticesId, new[] { "x" });
            VertexY = FindProperty(VerticesId, new[] { "y" });
            VertexZ = FindProperty(VerticesId, new[] { "z" });
            VertexIndices = FindProperty(FacesId, new[] { "vertex_indices", "vertex", "vertices", "vertex_indexes", "vertex_index", "index" });           
            int ni = NormalsId == -1 ? VerticesId : NormalsId;          
            NormalX = FindProperty(ni, new[] { "nx", "normal_x", "x_normal", "xnormal", "normalx" });
            NormalY = FindProperty(ni, new[] { "ny", "normal_y", "y_normal", "ynormal", "normaly" });
            NormalZ = FindProperty(ni, new[] { "nz", "normal_z", "z_normal", "znormal", "normalz"});
            if (NormalX != -1)
                NormalsId = ni;
        }
    }
}

