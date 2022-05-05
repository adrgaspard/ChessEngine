using System;
using System.Globalization;
using System.Windows.Data;

namespace ChessEngine.UI.WPF.Converters.Clock
{
    public class TimeSpanToClockStringConverter : IValueConverter
    {
        protected static readonly TimeSpan TwentySeconds = TimeSpan.FromSeconds(20);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan timeSpan = (TimeSpan)value;
            return timeSpan >= TwentySeconds ? $"{TimeSpan.FromSeconds(Math.Truncate(timeSpan.TotalSeconds)):mm\\:ss}" : $"{timeSpan:mm\\:ss\\.f}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
