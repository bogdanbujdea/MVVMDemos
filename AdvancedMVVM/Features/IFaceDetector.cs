using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Microsoft.ProjectOxford.Face.Contract;

namespace AdvancedMVVM.Features
{
    public interface IFaceDetector
    {
        Task<List<Face>> DetectFaces(SoftwareBitmap softwareBitmap);
        Task CreateFaceGroup();
    }
}