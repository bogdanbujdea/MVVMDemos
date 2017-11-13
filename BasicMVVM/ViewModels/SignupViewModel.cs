using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Windows.UI.Popups;
using BasicMVVM.Annotations;
using BasicMVVM.Models;

namespace BasicMVVM.ViewModels
{
    public class SignupViewModel: INotifyPropertyChanged
    {
        private SignupInfo _signupInfo;

        public SignupViewModel()
        {
            SignupCommand = new CommandExample(Signup);
            SignupInfo = new SignupInfo();
        }

        private async void Signup()
        {
           await new MessageDialog($"Your info is: username({SignupInfo.Username}), email({SignupInfo.Email}), password({SignupInfo.Password}).").ShowAsync();
        }

        public SignupInfo SignupInfo
        {
            get => _signupInfo;
            set
            {
                if (Equals(value, _signupInfo)) return;
                _signupInfo = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand SignupCommand { get; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CommandExample: ICommand
    {
        private readonly Action _action;

        public CommandExample(Action action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action();
        }

        public event EventHandler CanExecuteChanged;
    }
}
