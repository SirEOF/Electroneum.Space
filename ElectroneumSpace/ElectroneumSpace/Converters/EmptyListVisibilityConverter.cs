using System;
using System.Collections;
using System.Globalization;
using Xamarin.Forms;

namespace ElectroneumSpace.Converters
{
    public class EmptyListVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IList list)
                return list.Count == 0 ? true : false;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Convert(value, targetType, parameter, culture);
    }
}
