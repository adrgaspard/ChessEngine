using System;
using System.Globalization;
using System.Windows.Data;

namespace ChessEngine.UI.WPF.Converters.Clock
{
    public class RemainingTimeAndActivatedBoolToBrushConverter : IMultiValueConverter
    {
        protected static readonly TimeSpan TwentySeconds = TimeSpan.FromSeconds(20);

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan remainingTime = (TimeSpan)values[0];
            if (remainingTime <= TimeSpan.Zero)
            {
                return Brushes.DarkRedBrush;
            }
            bool isActivated = (bool)values[1];
            if (isActivated)
            {
                if (remainingTime < TwentySeconds)
                {
                    return (int)Math.Truncate(remainingTime.TotalSeconds) % 2 == 0 ? Brushes.MediumRedBrush : Brushes.LightRedBrush;
                }
            }
            return Brushes.WhiteBrush;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
