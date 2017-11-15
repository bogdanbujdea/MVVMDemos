using System.Threading.Tasks;
using Windows.Graphics.Imaging;

namespace AdvancedMVVM.ViewModels
{
    public interface ISignupViewModel
    {
        Task RetrieveFaces(SoftwareBitmap softwareBitmap, double heightScale, double widthScale);
        bool IsBusy { get; set; }
    }
}