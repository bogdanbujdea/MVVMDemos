using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Popups;
using BasicMVVM.Annotations;
using BasicMVVM.Models;

namespace BasicMVVM.ViewModels
{
    public class SignupViewModel: INotifyPropertyChanged
    {
        private string _email;
        private string _password;
        private string _username;

        public SignupViewModel()
        {
            SignupCommand = new RelayCommand(Signup, SignupCanExecute);
        }

        private bool SignupCanExecute()
        {
            return HasValidInfo();
        }

        private async void Signup()
        {
           await new MessageDialog($"Your info is: username({Username}), email({Email}), password({Password}).").ShowAsync();
        }

        public string Username
        {
            get => _username;
            set
            {
                if (value == _username) return;
                _username = value;
                OnPropertyChanged();
                SignupCommand.RaiseCanExecuteChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (value == _email) return;
                _email = value;
                OnPropertyChanged();
                SignupCommand.RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (value == _password) return;
                _password = value;
                OnPropertyChanged();
                SignupCommand.RaiseCanExecuteChanged();
            }
        }
        public RelayCommand SignupCommand { get; }

        private bool HasValidInfo()
        {
            return string.IsNullOrWhiteSpace(Username) == false &&
                   string.IsNullOrWhiteSpace(Email) == false &&
                   string.IsNullOrWhiteSpace(Password) == false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
