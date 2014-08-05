using System;
using System.Globalization;
using System.Windows.Data;

namespace UsedParts.UI.Converters
{
    public class InvertedBoolConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var b = (bool)value;
            return !b;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var b = (bool)value;
            return !b;
        }

        #endregion
    }
}
