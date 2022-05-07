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
            if (timeSpan == TimeSpan.MaxValue)
            {
                return "∞";
            }
            return timeSpan >= TwentySeconds || timeSpan == TimeSpan.Zero ? $"{TimeSpan.FromSeconds(Math.Truncate(timeSpan.TotalSeconds)):mm\\:ss}" : $"{timeSpan:mm\\:ss\\.f}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
