using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.UI.Popups;
using AdvancedMVVM.Controls;
using AdvancedMVVM.Features;
using AdvancedMVVM.Models;
using AdvancedMVVM.Tools;
using Caliburn.Micro;
using Microsoft.ProjectOxford.Face.Contract;

namespace AdvancedMVVM.ViewModels
{
    public class UsersViewModel : ViewModelBase, IHandleWithTask<UserInfo>
    {
        private readonly IFaceDetector _faceDetector;
        private readonly IFaceAnalyzer _faceAnalyzer;
        private ObservableCollection<FaceInfo> _faces;
        private PresentationStatistics _statistics;
        private SoftwareBitmap _lastFrame;
        private NewUserControlViewModel _newUserControlViewModel;
        private ObservableCollection<UserInfo> _users;

        public UsersViewModel(IFaceDetector faceDetector, IFaceAnalyzer faceAnalyzer, IEventAggregator eventAggregator, NewUserControlViewModel newUserControlViewModel)
        {
            _faceDetector = faceDetector;
            _faceAnalyzer = faceAnalyzer;
            Statistics = new PresentationStatistics();
            NewUserControlViewModel = newUserControlViewModel;
            NewUserControlViewModel.UserCreated += NewUserControlViewModel_UserCreated;
            Users = new ObservableCollection<UserInfo>();
            eventAggregator.Subscribe(this);
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

        public NewUserControlViewModel NewUserControlViewModel
        {
            get => _newUserControlViewModel;
            set
            {
                if (Equals(value, _newUserControlViewModel)) return;
                _newUserControlViewModel = value;
                NotifyOfPropertyChange(() => NewUserControlViewModel);
            }
        }

        public async Task RetrieveFaces(SoftwareBitmap softwareBitmap, double heightScale, double widthScale)
        {
            _lastFrame = softwareBitmap;
            var faces = await _faceDetector.DetectFaces(softwareBitmap);
            Execute.OnUIThread(async () =>
            {
                var processedFaces = _faceAnalyzer.ProcessFaces(faces, heightScale, widthScale);
                Faces = new ObservableCollection<FaceInfo>(processedFaces);
                if (faces.Count == 0)
                {
                    Statistics = new PresentationStatistics();
                    return;
                }
                Statistics.DetectedFaces = faces.Count;
                Statistics.AngryUsers = faces.Count(f => f.FaceAttributes.Emotion.Anger > 0.7);
                Statistics.HappyUsers = faces.Count(f => f.FaceAttributes.Emotion.Happiness > 0.7);
                Statistics.NeutralUsers = faces.Count(f => f.FaceAttributes.Emotion.Neutral > 0.7);
                Statistics.UsersWithGlasses = faces.Count(f => f.FaceAttributes.Glasses != Glasses.NoGlasses);
                Statistics.AgeAverage = faces.Average(f => f.FaceAttributes.Age);
                await AddHappyPeople();
            });
        }

        private async Task AddHappyPeople()
        {
            if (Faces.Count == 0)
                return;
            var tempFrame = _lastFrame;
            foreach (var faceInfo in Faces.Where(f => f.EmotionType == EmotionType.Happy))
            {
                Users.Add(new UserInfo
                {
                    Email = "test@test.com",
                    Username = $"{faceInfo.Age} - {faceInfo.Glasses} - {faceInfo.EmotionType}",
                    Image = await ImageCropper.CropFaceFromImage(tempFrame, faceInfo.OriginalFaceRectangle)
                });
            }
        }

        public async Task UserFaceSelected(FaceInfo faceInfo)
        {
            var imageSource = await ImageCropper.CropFaceFromImage(_lastFrame, faceInfo.OriginalFaceRectangle);
            NewUserControlViewModel.UserImage = imageSource;
        }

        private void NewUserControlViewModel_UserCreated(object sender, UserInfo e)
        {
            Users.Add(e);
        }

        public async Task Handle(UserInfo message)
        {
            await new MessageDialog($"Your info is: username({message.Username}), email({message.Email}), password({message.Password}).").ShowAsync();
        }
    }
}
