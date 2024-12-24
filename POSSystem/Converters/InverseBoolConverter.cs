using Microsoft.UI.Xaml.Data;
using System;

namespace POSSystem.Converters
{
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolean)
            {
                return !boolean;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolean)
            {
                return !boolean;
            }

            return value;
        }
    }
}