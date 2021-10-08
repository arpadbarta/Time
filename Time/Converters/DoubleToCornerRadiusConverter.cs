using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Time.Converters
{
    public class DoubleToCornerRadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is double doubleValue ? new CornerRadius(doubleValue) : new CornerRadius();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
