using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Watchlist.Converter
{
    [ValueConversion(typeof(object), typeof(Color))]
    internal class TextBlockForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (string)value == "true";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => (bool)value ? "true" : "false";
    }
}
