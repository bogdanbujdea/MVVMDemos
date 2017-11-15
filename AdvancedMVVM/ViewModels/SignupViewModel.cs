using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.UI.Popups;
using AdvancedMVVM.Features;
using AdvancedMVVM.Models;
using AdvancedMVVM.Tools;
using Caliburn.Micro;
using Microsoft.ProjectOxford.Face.Contract;

namespace AdvancedMVVM.ViewModels
{
    public class SignupViewModel : ViewModelBase, ISignupViewModel
    {
        private readonly IFaceDetector _faceDetector;
        private readonly IFaceAnalyzer _faceAnalyzer;
        private ObservableCollection<FaceInfo> _faces;
        private PresentationStatistics _statistics;
        private SoftwareBitmap _lastFrame;
        private INewUserControlViewModel _newUserControlViewModel;
        private IUsersListViewModel _usersListViewModel;

        public SignupViewModel(IFaceDetector faceDetector, IFaceAnalyzer faceAnalyzer, NewUserControlViewModel newUserControlViewModel, UsersListViewModel usersListViewModel)
        {
            _faceDetector = faceDetector;
            _faceAnalyzer = faceAnalyzer;
            Statistics = new PresentationStatistics();
            NewUserControlViewModel = newUserControlViewModel;
            NewUserControlViewModel.UserCreated += NewUserControlViewModel_UserCreated;
            UsersListViewModel = usersListViewModel;
        }


        public ObservableCollection<FaceInfo> Faces
        {
            get => _faces;
            set
            {
                if (Equals(value, _faces)) return;
                _faces = value;
                NotifyOfPropertyChange(() => Faces);
            }
        }

        public PresentationStatistics Statistics
        {
            get => _statistics;
            set
            {
                if (Equals(value, _statistics)) return;
                _statistics = value;
                NotifyOfPropertyChange(() => Statistics);
            }
        }

        public INewUserControlViewModel NewUserControlViewModel
        {
            get => _newUserControlViewModel;
            set
            {
                if (Equals(value, _newUserControlViewModel)) return;
                _newUserControlViewModel = value;
                NotifyOfPropertyChange(() => NewUserControlViewModel);
            }
        }

        public IUsersListViewModel UsersListViewModel
        {
            get => _usersListViewModel;
            set
            {
                if (Equals(value, _usersListViewModel)) return;
                _usersListViewModel = value;
                NotifyOfPropertyChange(() => UsersListViewModel);
            }
        }
        
        public async Task RetrieveFaces(SoftwareBitmap softwareBitmap, double heightScale, double widthScale)
        {
            _lastFrame = softwareBitmap;
            var faces = await _faceDetector.DetectFaces(softwareBitmap);
            Execute.OnUIThread(async () =>
            {
                Statistics.CallCount++;
                var processedFaces = _faceAnalyzer.ProcessFaces(faces, heightScale, widthScale);
                Faces = new ObservableCollection<FaceInfo>(processedFaces);
                if (faces.Count == 0)
                {
                    return;
                }
                Statistics.DetectedFaces = faces.Count;
                Statistics.AngryUsers = faces.Count(f => f.FaceAttributes.Emotion.Anger > 0.7);
                Statistics.HappyUsers = faces.Count(f => f.FaceAttributes.Emotion.Happiness > 0.7);
                Statistics.NeutralUsers = faces.Count(f => f.FaceAttributes.Emotion.Neutral > 0.7);
                Statistics.UsersWithGlasses = faces.Count(f => f.FaceAttributes.Glasses != Glasses.NoGlasses);
                Statistics.AgeAverage = faces.Average(f => f.FaceAttributes.Age);
                Statistics.TotalHappyUsers += Statistics.HappyUsers;
                await AddHappyPeople();
            });
        }

        private async Task AddHappyPeople()
        {
            if (Faces.Count == 0)
                return;
            await UsersListViewModel.AddUsers(Faces.Where(f => f.EmotionType == EmotionType.Happy), _lastFrame);
        }

        public async Task UserFaceSelected(FaceInfo faceInfo)
        {
            var imageSource = await ImageCropper.CropFaceFromImage(_lastFrame, faceInfo.OriginalFaceRectangle);
            NewUserControlViewModel.UserImage = imageSource;
        }

        private async void NewUserControlViewModel_UserCreated(object sender, UserInfo userInfo)
        {
            await new MessageDialog($"Your info is: username({userInfo.Username}), email({userInfo.Email}), password({userInfo.Password}).").ShowAsync();
        }
    }
}
