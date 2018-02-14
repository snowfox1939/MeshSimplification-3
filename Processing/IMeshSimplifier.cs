using Polynano.Common;

namespace Polynano.Processing
{
    internal interface IMeshSimplifier
    {
        ContractionHistory SimplifyOneStep();

        void RevertOneStep();

        MeshGeometryData GetModel();
    }
}
