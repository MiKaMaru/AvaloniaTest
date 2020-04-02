using Avalonia.Data.Converters;
using System;
using System.Globalization;
using SysConvert = System.Convert;

namespace AvaloniaXmlLoadTest.Utils
{
    public class WidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double subtrahend = 0;
            if (parameter != null)
            {
                subtrahend = SysConvert.ToDouble(parameter);
            }

            return (double)value - subtrahend;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value;
        }
    }
    public class DividedWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double subtrahend = 0, divider = 1;
            if (parameter != null)
            {
                var subsplitted = parameter.ToString().Split("_");
                subtrahend = SysConvert.ToDouble(subsplitted[0]);
                divider = SysConvert.ToDouble(subsplitted[1]);
            }

            return ((double)value - subtrahend) / divider;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value;
        }
    }

    public class LogicalNegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value;
        }
    }
}
