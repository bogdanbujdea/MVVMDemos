using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using AdvancedMVVM.Models;
using AdvancedMVVM.Tools;
using Caliburn.Micro;

namespace AdvancedMVVM.ViewModels
{
    public class UsersListViewModel: ViewModelBase, IHandle<UserInfo>, IUsersListViewModel
    {
        private ObservableCollection<UserInfo> _users;

        public UsersListViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.Subscribe(this);
            Users = new ObservableCollection<UserInfo>();
        }

        public ObservableCollection<UserInfo> Users
        {
            get => _users;
            set
            {
                if (Equals(value, _users)) return;
                _users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }

        public async Task AddUsers(IEnumerable<FaceInfo> faces, SoftwareBitmap softwareBitmap)
        {
            foreach (var faceInfo in faces)
            {
                Users.Add(new UserInfo
                {
                    Email = "test@test.com",
                    Username = $"{faceInfo.Age} - {faceInfo.Glasses} - {faceInfo.EmotionType}",
                    Image = await ImageCropper.CropFaceFromImage(softwareBitmap, faceInfo.OriginalFaceRectangle)
                });
            }
        }

        public void Handle(UserInfo userInfo)
        {
            Users.Add(userInfo);
        }
    }
}
