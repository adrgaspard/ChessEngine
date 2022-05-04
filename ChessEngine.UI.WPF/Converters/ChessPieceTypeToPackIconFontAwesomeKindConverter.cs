using ChessEngine.Core.Environment;
using MahApps.Metro.IconPacks;
using System;
using System.Globalization;
using System.Windows.Data;

namespace ChessEngine.UI.WPF.Converters
{
    public class ChessPieceTypeToPackIconFontAwesomeKindConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PieceType chessPieceType)
            {
                return chessPieceType switch
                {
                    PieceType.King => PackIconFontAwesomeKind.ChessKingSolid,
                    PieceType.Pawn => PackIconFontAwesomeKind.ChessPawnSolid,
                    PieceType.Knight => PackIconFontAwesomeKind.ChessKnightSolid,
                    PieceType.Bishop => PackIconFontAwesomeKind.ChessBishopSolid,
                    PieceType.Rook => PackIconFontAwesomeKind.ChessRookSolid,
                    PieceType.Queen => PackIconFontAwesomeKind.ChessQueenSolid,
                    _ => throw new NotSupportedException($"The value {chessPieceType} is not supported."),
                };
            }
            throw new ArgumentException($"The value is not a {nameof(PieceType)}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
