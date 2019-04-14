using Polynano;
using Polynano.Graphics;
using Polynano.IO;
using Polynano.UI;

namespace Polynano.UI
{
    public class MainApplication
    {
        Viewer _viewer;

        Application _application;

        public MainApplication(Application app)
        {
            _application = app;
            _viewer = new Viewer();
            load();
        }

        private void load()
        {
            _application.Load("../../models/cow.ply");
            _application.InitializeSimplifier();
            var currentModel = _application.Simplifier.GetModel();
            _viewer.Transformation = VertexNormalizer.GetNormalizingMatrix(currentModel.Vertices);
            _viewer.SetMesh(currentModel);
            _viewer.DisplayFaces = true;
            _viewer.Run();
        }
    }
}