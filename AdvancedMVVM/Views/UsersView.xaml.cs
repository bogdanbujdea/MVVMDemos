using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Graphics.Display;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.System.Display;
using Windows.UI.Xaml;
using AdvancedMVVM.ViewModels;

namespace AdvancedMVVM.Views
{
    public sealed partial class UsersView
    {
        private readonly DispatcherTimer _dispatcherTimer;
        private readonly DisplayRequest _displayRequest;
        private readonly MediaCapture _mediaCapture;

        public UsersView()
        {
            InitializeComponent();
            _displayRequest = new DisplayRequest();
            _mediaCapture = new MediaCapture();
            _dispatcherTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            _dispatcherTimer.Tick += TimerTick;
            Loaded += ViewLoaded;
        }

        public UsersViewModel ViewModel => DataContext as UsersViewModel;

        private async void TimerTick(object sender, object e)
        {
            var lowLagCapture =
                await _mediaCapture.PrepareLowLagPhotoCaptureAsync(ImageEncodingProperties.CreateUncompressed(MediaPixelFormat.Bgra8));
            var capturedPhoto = await lowLagCapture.CaptureAsync();
            var softwareBitmap = capturedPhoto.Frame.SoftwareBitmap;
            await lowLagCapture.FinishAsync();
            var widthScale = CameraGrid.ActualWidth / softwareBitmap.PixelWidth;
            var heightScale = CameraGrid.ActualHeight / softwareBitmap.PixelHeight;
            await ViewModel.RetrieveFaces(softwareBitmap, heightScale, widthScale);
        }

        private async void ViewLoaded(object sender, RoutedEventArgs e)
        {
            await StartPreviewAsync();
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
            // Get available devices for capturing pictures
            DeviceInformationCollection allVideoDevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            // Get the desired camera by panel
            DeviceInformation desiredDevice = allVideoDevices.LastOrDefault();
            // If there is no device mounted on the desired panel, return the first device found
            return desiredDevice ?? allVideoDevices.FirstOrDefault();
        }
    }
}