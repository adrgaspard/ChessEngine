using ChessEngine.Core.Environment;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ChessEngine.UI.WPF.Converters.Clock
{
    public class BlackColourToVisibleCollapsedVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Colour.Black ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
