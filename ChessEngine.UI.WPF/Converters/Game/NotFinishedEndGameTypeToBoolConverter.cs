using ChessEngine.Core.Match;
using System;
using System.Globalization;
using System.Windows.Data;

namespace ChessEngine.UI.WPF.Converters.Game
{
    public class NotFinishedEndGameTypeToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is EndGameType.GameIsNotFinished;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
