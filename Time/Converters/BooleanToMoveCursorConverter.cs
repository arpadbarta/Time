using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace Time.Converters
{
    public class BooleanToMoveCursorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is true ? Cursors.SizeAll : Cursors.Arrow;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
