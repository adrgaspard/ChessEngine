﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ChessEngine.UI.WPF.Converters
{
    public class ObjectToVisibleCollapsedVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}