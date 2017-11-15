using AdvancedMVVM.ViewModels;

namespace AdvancedMVVM.Controls
{
    public sealed partial class NewUserControl
    {
        public NewUserControl()
        {
            InitializeComponent();
            Loaded += NewUserControl_Loaded;
        }

        private void NewUserControl_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var newUserControlViewModel = DataContext as NewUserControlViewModel;
        }
    }
}