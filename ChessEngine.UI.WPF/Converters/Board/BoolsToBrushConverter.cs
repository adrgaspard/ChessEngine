using System;
using System.Globalization;
using System.Windows.Data;

namespace ChessEngine.UI.WPF.Converters.Board
{
    public class BoolsToBrushConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool positionIsEven = (bool)values[0];
            if ((bool)values[1])
            {
                return positionIsEven ? Brushes.SelectedDarkColorBrush : Brushes.SelectedLightColorBrush;
            }
            if ((bool)values[2])
            {
                return positionIsEven ? Brushes.HighlightedDarkColorBrush : Brushes.HighlightedLightColorBrush;
            }
            if ((bool)values[3])
            {
                return positionIsEven ? Brushes.MarkedDarkColorBrush : Brushes.MarkedLightColorBrush;
            }
            return positionIsEven ? Brushes.DarkColorBrush : Brushes.LightColorBrush;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
