using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using AdvancedMVVM.Models;

namespace AdvancedMVVM.ViewModels
{
    public interface IUsersListViewModel
    {
        ObservableCollection<UserInfo> Users { get; set; }
        Task AddUsers(IEnumerable<FaceInfo> faces, SoftwareBitmap softwareBitmap);
    }
}