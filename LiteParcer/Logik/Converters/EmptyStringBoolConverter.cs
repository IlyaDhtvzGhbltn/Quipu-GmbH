using System;
using System.Globalization;
using System.Windows.Data;

namespace LiteParcer.Logik.Converters
{
    class EmptyStringBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = (string)value;
            if (string.IsNullOrWhiteSpace(text))
                return false;
            else
                return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
