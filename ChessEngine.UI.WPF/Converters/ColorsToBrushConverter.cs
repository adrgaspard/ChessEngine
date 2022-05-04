using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace ChessEngine.UI.WPF.Converters
{
    public class ColorsToBrushConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            IList<Color> colors = values.Cast<Color>().ToList();
            Color result = colors.Any() ? colors.First() : Colors.Transparent;
            for (int i = 1; i < colors.Count; i++)
            {
                float add = (colors[i].A + 1f) / 256;
                result = result * (1 - add) + Color.FromArgb(0xFF, colors[i].R, colors[i].G, colors[i].B) * add;
            }
            result.Clamp();
            return new SolidColorBrush(result);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
