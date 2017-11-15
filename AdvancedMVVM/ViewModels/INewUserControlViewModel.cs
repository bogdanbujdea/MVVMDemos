using System;
using Windows.UI.Xaml.Media;
using AdvancedMVVM.Models;

namespace AdvancedMVVM.ViewModels
{
    public interface INewUserControlViewModel
    {
        event EventHandler<UserInfo> UserCreated;

        ImageSource UserImage { get; set; }
    }
}