using System;
using System.Globalization;
using System.Windows.Data;

namespace Watchlist.Converter
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (string)value == "true";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => (bool)value ? "true" : "false";
    }
}
