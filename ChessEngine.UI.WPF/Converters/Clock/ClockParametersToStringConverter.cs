using ChessEngine.MVVM.Models;
using ChessEngine.MVVM.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace ChessEngine.UI.WPF.Converters.Clock
{
    public class ClockParametersToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ClockParameters clockParameters = (ClockParameters)value;
            return clockParameters == ClockViewModel.InfiniteClockParameters ? "∞" : $"{clockParameters.BaseTime.TotalMinutes} + {clockParameters.IncrementTime.TotalSeconds}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
