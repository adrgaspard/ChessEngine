using ChessEngine.Core.Environment;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ChessEngine.UI.WPF.Converters
{
    public class ChessPieceColourToBrushConverter : IValueConverter
    {
        public static readonly SolidColorBrush Black = new(Colors.Black);
        public static readonly SolidColorBrush White = new(Color.FromRgb(0xC4, 0x2F, 0x2F)/*Color.FromRgb(0xa9, 0xa1, 0xad)*/);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Colour chessPieceColour)
            {
                return chessPieceColour switch
                {
                    Colour.White => White,
                    Colour.Black => Black,
                    _ => throw new NotSupportedException($"The value {chessPieceColour} is not supported."),
                };
            }
            throw new ArgumentException($"The value is not a {nameof(Colour)}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
