using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace UsedParts.UI.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public bool IsInverted { get; set; }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var b = (bool)value;
            if (IsInverted)
                b = !b;
            return b ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var res = (Visibility)value == Visibility.Visible;
            if (parameter != null)
                res = !res;
            return res;
        }

        #endregion
    }
}

