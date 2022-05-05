using ChessEngine.Core.Environment;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ChessEngine.UI.WPF.Converters
{
    public class PositionToInvertedColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Position chessPosition)
            {
                return new SolidColorBrush((chessPosition.File + chessPosition.Rank) % 2 == 0 ? PositionToColorConverter.LightColor : PositionToColorConverter.DarkColor);
            }
            throw new ArgumentException($"The value is not a {nameof(Position)}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
