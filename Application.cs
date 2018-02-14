using Polynano.IO;
using System;
using System.IO;
using System.Linq;
using OpenTK;
using Polynano.Common;
using Polynano.Processing;
using static System.String;

namespace Polynano
{
    internal class Application
    {
        /// <summary>
        /// The original model that was loaded from file
        /// </summary>
        public MeshData OriginalModel { get; private set; }

        /// <summary>
        /// Get the mesh simplifier.
        /// </summary>
        public MeshSimplifier Simplifier { get; private set; }

        /// <summary>
        /// Load a model from a file
        /// </summary>
        /// <param name="filepath">name of the file to load</param>
        /// <param name="progressReporter">progress reporter to report operation progress back to UI</param>
        /// <returns>full path of the file that was loaded</returns>
        public string Load(string filepath, IProgress<int> progressReporter = null)
        {
            if (IsNullOrEmpty(filepath))
                return null;

            var modelFileName = Path.GetFileNameWithoutExtension(filepath);
            using (var stream = new FileStream(filepath, FileMode.Open))
            {
                OriginalModel = ModelLoader.Load(stream, progressReporter);
                stream.Close();
            }

            return modelFileName;
        }

        public void InitializeSimplifier()
        {
            Simplifier = new MeshSimplifier();
            Simplifier.SetStructure(OriginalModel);
        }

        /// <summary>
        /// Save a model to a file
        /// </summary>
        /// <param name="filepath">the file name to save the model to</param>
        /// <param name="progressReporter">progress Reporter to report the operation progress back to UI</param>
        public void Save(string filepath, IProgress<int> progressReporter = null)
        {
            var m = Simplifier.GetModel();

            var model = new MeshData
            {
                Vertices = new Vector3[m.ActiveVertices.Length],
                Faces = m.Faces
            };

            int i = 0;
            foreach (var v in m.ActiveVertices)
            {
                model.Vertices[i] = OriginalModel.Vertices[v];
                i++;
            }

            for (int j = 0; j < model.Faces.Length; j++)
            {
                var v1 = Array.BinarySearch(m.ActiveVertices, model.Faces[j].Vertex1);
                var v2 = Array.BinarySearch(m.ActiveVertices, model.Faces[j].Vertex2);
                var v3 = Array.BinarySearch(m.ActiveVertices, model.Faces[j].Vertex3);

                model.Faces[j] = new IndexedTriangle(v1, v2, v3);
            }

            ModelSaver.Save(filepath, model, progressReporter);
        }
    }
}
