using ChessEngine.Core.Match;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ChessEngine.UI.WPF.Converters.Game
{
    public class FinishedEndGameTypeToVisibleCollapsedVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((EndGameType)value & EndGameType.GameIsFinished) != EndGameType.GameIsNotFinished ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
