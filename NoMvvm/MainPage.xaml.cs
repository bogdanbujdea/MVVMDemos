using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            await new MessageDialog($"Your info is: username({Username.Text}), email({Email.Text}), password({Password.Text}).")
                .ShowAsync();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Username.Text) ||
                string.IsNullOrWhiteSpace(Email.Text) ||
                string.IsNullOrWhiteSpace(Password.Text))
            {
                SignupButton.IsEnabled = false;
            }
            else
            {
                SignupButton.IsEnabled = true;
            }
        }
    }
}