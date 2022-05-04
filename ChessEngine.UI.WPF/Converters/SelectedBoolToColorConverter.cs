using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ChessEngine.UI.WPF.Converters
{
    public class SelectedBoolToColorConverter : IValueConverter
    {
        public static readonly Color SelectedColor = Color.FromArgb(0xFF, 0x40, 0xCF, 0xCF);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? SelectedColor : Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
