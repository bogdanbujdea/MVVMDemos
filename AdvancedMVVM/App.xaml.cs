using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Activation;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using AdvancedMVVM.Features;
using AdvancedMVVM.ViewModels;
using AdvancedMVVM.Views;
using Caliburn.Micro;

namespace AdvancedMVVM
{
    sealed partial class App
    {
        private WinRTContainer _container;

        public App()
        {
            Initialize();
            InitializeComponent();
            UnhandledException += App_UnhandledException;
        }

        private async void App_UnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            await new MessageDialog(e.Message).ShowAsync();
        }

        protected override void Configure()
        {
            _container = new WinRTContainer();

            _container.RegisterWinRTServices();
            _container.RegisterPerRequest(typeof(IFaceDetector), "IFaceDetector", typeof(FaceDetector));
            _container.RegisterPerRequest(typeof(IFaceAnalyzer), "IFaceAnalyzer", typeof(FaceAnalyzer));
            _container.PerRequest<UsersListViewModel>();
            _container.PerRequest<NewUserControlViewModel>();
            _container.PerRequest<SignupViewModel>();

            AddCustomConventions();
        }

        static void AddCustomConventions()
        {
            ConventionManager.AddElementConvention<TextBox>(TextBox.TextProperty, "Text", "TextChanged");
            ConventionManager.AddElementConvention<ButtonBase>(ButtonBase.ContentProperty, "DataContext", "Click");
        }
        protected override void PrepareViewFirst(Frame rootFrame)
        {
            _container.RegisterNavigationService(rootFrame);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (args.PreviousExecutionState == ApplicationExecutionState.Running)
                return;

            DisplayRootView<SignupView>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
