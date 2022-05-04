using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ChessEngine.UI.WPF.Converters
{
    public class MarkedBoolToColorConverter : IValueConverter
    {
        public static readonly Color MarkedColor = Color.FromArgb(0xC0, 0xFC, 0xE8, 0x4C);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? MarkedColor : Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
