using ElectroneumSpace.Constants;

using System;
using System.Globalization;

using Xamarin.Forms;

namespace ElectroneumSpace.Converters
{
    public class HomeSectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is int ? ((HomeSection)value).ToString() : string.Empty;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Convert(value, targetType, parameter, culture);
    }
}
