﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ChessEngine.UI.WPF.Converters
{
    public class BoolToVisibleCollapsedVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
