using ChessEngine.Core.Environment;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ChessEngine.UI.WPF.Converters
{
    public class ChessPositionToColorConverter : IValueConverter
    {
        public static readonly Color DarkColor = Color.FromRgb(0xA9, 0x7A, 0x65);
        public static readonly Color LightColor = Color.FromRgb(0xF1, 0xD9, 0xC8);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Position chessPosition)
            {
                return (chessPosition.File + chessPosition.Rank) % 2 == 0 ? DarkColor : LightColor;
            }
            throw new ArgumentException($"The value is not a {nameof(Position)}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
