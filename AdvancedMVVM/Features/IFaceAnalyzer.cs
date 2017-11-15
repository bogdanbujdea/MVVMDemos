using System.Collections.Generic;
using AdvancedMVVM.ViewModels;
using Microsoft.ProjectOxford.Face.Contract;

namespace AdvancedMVVM.Features
{
    public interface IFaceAnalyzer
    {
        List<FaceInfo> ProcessFaces(List<Face> faces, double heightScale, double widthScale);
    }
}
