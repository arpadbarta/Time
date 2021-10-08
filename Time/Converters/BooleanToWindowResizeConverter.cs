using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Time.Converters
{
    public class BooleanToWindowResizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is true ? ResizeMode.CanResizeWithGrip : ResizeMode.NoResize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
