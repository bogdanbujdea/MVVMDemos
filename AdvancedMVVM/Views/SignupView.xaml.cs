using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.System.Display;
using Windows.UI.Xaml;
using AdvancedMVVM.ViewModels;

namespace AdvancedMVVM.Views
{
    public sealed partial class SignupView
    {
        private readonly DispatcherTimer _dispatcherTimer;
        private readonly DispatcherTimer _photoTimer;
        private readonly DisplayRequest _displayRequest;
        private readonly MediaCapture _mediaCapture;
        private SoftwareBitmap _softwareBitmap;

        public SignupView()
        {
            InitializeComponent();
            _displayRequest = new DisplayRequest();
            _mediaCapture = new MediaCapture();
            _photoTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _dispatcherTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
            _dispatcherTimer.Tick += TimerTick;
            _photoTimer.Tick += PhotoTick;
            Loaded += ViewLoaded;
        }

        private async void PhotoTick(object sender, object e)
        {
            try
            {
                _softwareBitmap = await CapturePhoto();
            }
            catch (Exception)
            {
            }
        }

        public ISignupViewModel ViewModel => DataContext as ISignupViewModel;

        private async void TimerTick(object sender, object e)
        {
            try
            {
                ViewModel.IsBusy = true;
                var widthScale = CameraGrid.ActualWidth / _softwareBitmap.PixelWidth;
                var heightScale = CameraGrid.ActualHeight / _softwareBitmap.PixelHeight;
                await ViewModel.RetrieveFaces(_softwareBitmap, heightScale, widthScale);
            }
            catch (Exception)
            {
            }
            finally
            {
                ViewModel.IsBusy = false;
            }
        }

        private async Task<SoftwareBitmap> CapturePhoto()
        {
            var lowLagCapture =
                await _mediaCapture.PrepareLowLagPhotoCaptureAsync(ImageEncodingProperties.CreateUncompressed(MediaPixelFormat.Bgra8));
            var capturedPhoto = await lowLagCapture.CaptureAsync();
            var softwareBitmap = capturedPhoto.Frame.SoftwareBitmap;
            await lowLagCapture.FinishAsync();
            return softwareBitmap;
        }

        private async void ViewLoaded(object sender, RoutedEventArgs e)
        {
            await StartPreviewAsync();
            _photoTimer.Start();
            _dispatcherTimer.Start();
        }

        private async Task StartPreviewAsync()
        {
            _displayRequest.RequestActive();
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape;
            DeviceInformation cameraDevice = await FindCameraDeviceByPanelAsync();
            MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings { VideoDeviceId = cameraDevice.Id, StreamingCaptureMode = StreamingCaptureMode.Video };
            await _mediaCapture.InitializeAsync(settings);
            try
            {
                PreviewControl.Source = _mediaCapture;
                await _mediaCapture.StartPreviewAsync();
            }
            catch (FileLoadException)
            {
            }
        }

        private async Task<DeviceInformation> FindCameraDeviceByPanelAsync()
        {
            DeviceInformationCollection allVideoDevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            DeviceInformation desiredDevice = allVideoDevices.LastOrDefault();
            return desiredDevice ?? allVideoDevices.FirstOrDefault();
        }
    }
}