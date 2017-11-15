using System;
using Caliburn.Micro;

namespace AdvancedMVVM.ViewModels
{
    public class ViewModelBase: Screen
    {
        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (value == _isBusy) return;
                _isBusy = value;
                NotifyOfPropertyChange(() => IsBusy);
            }
        }
    }
}
