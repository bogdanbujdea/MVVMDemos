using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using AdvancedMVVM.ViewModels;

namespace AdvancedMVVM.Converters
{
    public class EmotionToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var emotion = (EmotionType)value;
            switch (emotion)
            {
                case EmotionType.Happy:
                    return new SolidColorBrush(Colors.Green);
                case EmotionType.Angry:
                    return new SolidColorBrush(Colors.Red);
                case EmotionType.Neutral:
                    return new SolidColorBrush(Colors.White);
                default:
                    return new SolidColorBrush(Colors.Black);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
