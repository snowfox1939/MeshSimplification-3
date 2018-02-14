using Polynano.IO.Ply;
using System.IO;
using OpenTK;
using Polynano.Common;
using System;

namespace Polynano.IO
{
    internal static class ModelSaver
    {
        /// <summary>
        /// Save a MeshData model into a file on disk
        /// </summary>
        /// <param name="filepath">path of the file to save the model to</param>
        /// <param name="model">the model to save</param>
        /// <param name="progressReporter">progress reporter, will be called in a undefined interval to report progress of the saving operation</param>
        public static void Save(string filepath, MeshData model, IProgress<int> progressReporter = null)
        {
            var elements = new[]
            {
                new Element("vertex", model.Vertices.Length, new[]
                {
                    new Property("x", PlyType.Float),
                    new Property("y", PlyType.Float),
                    new Property("z", PlyType.Float)
                }),
                new Element("face", model.Faces.Length, new[]
                {
                    new Property("vertex_index", PlyType.Int, PlyType.Int)
                })
            };

            var header = new Header(Format.Ascii, "Simplified by polynano", null, elements);
            var writer = new Writer(new FileStream(filepath, FileMode.Create), header);

            long totalToWrite = model.Faces.Length + model.Faces.Length;
            long written = 0;

            int lastReportedProgress = 10;

            // Go though the vertices and write them to the file.
            foreach (Vector3 vertex in model.Vertices)
            {
                writer.WriteValues(vertex.X, vertex.Y, vertex.Z);
                written++;

                // report progress on every 25% percent
                int progress = (int)(written * 100 / totalToWrite);
                if(progress > lastReportedProgress + 25 && progressReporter != null)
                {
                    progressReporter.Report(progress);
                    lastReportedProgress = progress;
                }
            }        

            foreach (IndexedTriangle face in model.Faces)
            {
                writer.WriteList(face.Vertex1, face.Vertex2, face.Vertex3);
                written++;
                int progress = (int)(written * 100 / totalToWrite);
                if (progress > lastReportedProgress + 25 && progressReporter != null)
                {
                    progressReporter.Report(progress);
                    lastReportedProgress = progress;
                }
            }

            progressReporter?.Report(100);
            writer.Close();
        }

    }
}
