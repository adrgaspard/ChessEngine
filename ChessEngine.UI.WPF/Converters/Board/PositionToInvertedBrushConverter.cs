using ChessEngine.Core.Environment;
using System;
using System.Globalization;
using System.Windows.Data;

namespace ChessEngine.UI.WPF.Converters.Board
{
    public class PositionToInvertedBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Position position)
            {
                return (position.File + position.Rank) % 2 == 0 ? Brushes.LightColorBrush : Brushes.DarkColorBrush;
            }
            throw new ArgumentException($"The value is not a {nameof(Position)}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
