using System;
using Windows.UI.Xaml.Media;
using AdvancedMVVM.Models;

namespace AdvancedMVVM.ViewModels
{
    public class NewUserControlViewModel : ViewModelBase, INewUserControlViewModel
    {
        private ImageSource _userImage;
        private string _email;
        private string _password;
        private string _username;

        public ImageSource UserImage
        {
            get => _userImage;
            set
            {
                if (Equals(value, _userImage)) return;
                _userImage = value;
                NotifyOfPropertyChange(() => UserImage);
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                if (value == _username) return;
                _username = value;
                NotifyOfPropertyChange(() => Username);
                NotifyOfPropertyChange(() => CanSignup);
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (value == _email) return;
                _email = value;
                NotifyOfPropertyChange(() => Email);
                NotifyOfPropertyChange(() => CanSignup);
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (value == _password) return;
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanSignup);
            }
        }

        public bool CanSignup => HasValidInfo();

        public void Signup()
        {
            OnUserCreated(new UserInfo{Email = Email, Password = Password, Username = Username, Image = UserImage});
        }

        public event EventHandler<UserInfo> UserCreated;

        private bool HasValidInfo()
        {
            return string.IsNullOrWhiteSpace(Username) == false &&
            string.IsNullOrWhiteSpace(Email) == false &&
            string.IsNullOrWhiteSpace(Password) == false;
        }

        protected virtual void OnUserCreated(UserInfo e)
        {
            UserCreated?.Invoke(this, e);
        }
    }
}
