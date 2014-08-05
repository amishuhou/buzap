using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace UsedParts.UI.Converters
{
    public class OrderColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isFavorite = (bool) value;
            return isFavorite
                ? Application.Current.Resources["PhoneAccentBrush"]
                : Application.Current.Resources["PhoneForegroundBrush"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
