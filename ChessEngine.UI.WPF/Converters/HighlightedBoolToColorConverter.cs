using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ChessEngine.UI.WPF.Converters
{
    public class HighlightedBoolToColorConverter : IValueConverter
    {
        public static readonly Color HighlightedColor = Color.FromArgb(0xE0, 0x34, 0x66, 0xE3);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? HighlightedColor : Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
