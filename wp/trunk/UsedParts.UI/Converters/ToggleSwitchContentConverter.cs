using System;
using System.Windows.Data;
using UsedParts.Localization;

namespace UsedParts.UI.Converters
{
    public class ToggleSwitchContentConverter : IValueConverter
    {
        private readonly string _falseValue = Resources.Off;
        private readonly string _trueValue = Resources.On;

        public object Convert(object value, Type targetType, object parameter,
              System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return _falseValue;
            return ("On".Equals(value)) ? _trueValue : _falseValue;
        }

        public object ConvertBack(object value, Type targetType,
               object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null && value.Equals(_trueValue);
        }
    }
}
