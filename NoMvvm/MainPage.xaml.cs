using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace NoMvvm
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void SignupButtonClicked(object sender, RoutedEventArgs e)
        {
           await new MessageDialog($"Your info is: username({Username.Text}), email({Email.Text}), password({Password.Text}).").ShowAsync();

        }
    }
}
