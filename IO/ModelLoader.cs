using System;
using System.IO;
using Polynano.IO.Ply;
using Polynano.Common;
using OpenTK;
using System.Collections.Generic;

using EarClipperLib;

namespace Polynano.IO
{
    internal static class ModelLoader
    {
        /// <summary>
        /// Parse a stream and get the Model from it
        /// </summary>
        /// <param name="s">stream to parse the model from</param>
        /// <param name="progressReporter">progressReporter to call in order to report the progress of the loading operation</param>
        /// <returns>parsed Model</returns>
        public static MeshData Load(Stream s, IProgress<int> progressReporter)
        {
            var reader = new Reader(s);
            var head = new NormalizedHeader(reader.ReadHeader());
            var elm = head.BaseHeader.Elements;

            var faces = new List<IndexedTriangle>(elm[head.FacesId].Count);

            var model = new MeshData
            {
                Vertices = new Vector3[elm[head.VerticesId].Count],
                Faces = null,
                Normals = new Vector3[head.NormalsId == -1 ? 0 : elm[head.NormalsId].Count]
            };

            int currVertex = 0;
            int currVertexPropWritten = 0;
            int currNormal = 0;
            int currNormalPropWritten = 0;
            int currFace = 0;

            // calling progressReporter.Report is expensive, limit it.
            long totalLoad = model.Vertices.Length + faces.Capacity + model.Normals.Length;
            int lastProgressReported = 10;

            // since the structure of the file is not known
            // and will be determined at the runtime
            // we don't know what elements and properties come first.
            // not the prettiest but definitely the fastest way to do this.
            for (int i = 0; i < elm.Count; ++i)
            {
                for (int j = 0; j < elm[i].Count; ++j)
                {
                    // call the progressReporter function on every 10% progress
                    int progress = (int)((currVertex + currNormal + currFace) * 100 / totalLoad);
                    if (progress > lastProgressReported + 10 && progressReporter != null)
                    {
                        progressReporter.Report(progress);
                        lastProgressReported = progress;
                    }

                    // go through every property of the current element
                    for (int k = 0; k < elm[i].Properties.Count; ++k)
                    {
                        bool ignored = true;

                        // if the current element was found by the normalizedHeader 
                        // to be a vertex element, parse the vertices
                        if (i == head.VerticesId)
                        {
                            if (k == head.VertexX)
                            {
                                model.Vertices[currVertex].X = reader.ReadProperty<float>();
                                currVertexPropWritten++;
                                ignored = false;
                            }
                            else if (k == head.VertexY)
                            {
                                model.Vertices[currVertex].Y = reader.ReadProperty<float>();
                                currVertexPropWritten++;
                                ignored = false;

                            }
                            else if (k == head.VertexZ)
                            {
                                model.Vertices[currVertex].Z = reader.ReadProperty<float>();
                                currVertexPropWritten++;
                                ignored = false;
                            }
                            if (currVertexPropWritten == 3)
                            {
                                currVertexPropWritten = 0;
                                currVertex++;
                            }
                        }
                        // if it is a normal element..
                        if (i == head.NormalsId)
                        {
                            if (k == head.NormalX)
                            {
                                model.Normals[currNormal].X = reader.ReadProperty<float>();
                                currNormalPropWritten++;
                                ignored = false;
                            }
                            else if (k == head.NormalY)
                            {
                                model.Normals[currNormal].X = reader.ReadProperty<float>();
                                currNormalPropWritten++;
                                ignored = false;
                            }
                            else if (k == head.NormalZ)
                            {
                                model.Normals[currNormal].X = reader.ReadProperty<float>();
                                currNormalPropWritten++;
                                ignored = false;
                            }
                            if (currNormalPropWritten == 3)
                            {
                                currNormal++;
                                currNormalPropWritten = 0;
                            }
                        }
                        // if it is a face element..
                        if (i == head.FacesId)
                        {
                            if (k == head.VerticesId)
                            {
                                // read the vertices of a face
                                List<int> verts = new List<int>(3);
                                foreach (int index in reader.ReadList<int>())
                                {
                                    verts.Add(index);
                                }

                                // if we got 3 vertices, its a normal triangle
                                // and the triangle to the list
                                if (verts.Count == 3)
                                {
                                    var t = new IndexedTriangle
                                    {
                                        Vertex1 = verts[0],
                                        Vertex2 = verts[1],
                                        Vertex3 = verts[2]
                                    };
                                    faces.Add(t);
                                }
                                else if (verts.Count == 4)
                                {
                                    faces.Add(new IndexedTriangle(verts[0], verts[1], verts[3]));
                                    faces.Add(new IndexedTriangle(verts[1], verts[2], verts[3]));
                                }
                                else
                                {
                                    // if we got more than 4 its a polygon we need to triangulate 
                                    // with the EarClipperLib Library.

                                    var points = new List<Vector3m>(verts.Count);

                                    foreach (var v in verts)
                                    {
                                        var vx = model.Vertices[v].X;
                                        var vy = model.Vertices[v].Y;
                                        var vz = model.Vertices[v].Z;
                                        points.Add(new Vector3m(vx, vy, vz));
                                    }

                                    var clipper = new EarClipping();
                                    clipper.SetPoints(points);
                                    clipper.Triangulate();
                                    var res = clipper.Result;

                                    for (int ri = 0; ri < res.Count; ri += 3)
                                    {
                                        faces.Add(new IndexedTriangle(points.IndexOf(res[ri]), points.IndexOf(res[ri + 1]), points.IndexOf(res[ri + 2])));
                                    }
                                }
                                currFace++;
                                ignored = false;
                            }
                        }
                        if (ignored)
                        {
                            reader.SkipProperty();
                        }
                    }
                }
            }

            model.Faces = faces.ToArray();

            if (model.Normals.Length == 0)
                model.Normals = null;

            progressReporter?.Report(100);
            return model;
        }
    }
}

