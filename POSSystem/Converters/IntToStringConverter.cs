﻿using Microsoft.UI.Xaml.Data;
using System;

namespace POSSystem.Converters
{
    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (int.TryParse(value as string, out var result))
            {
                return result;
            }
            return null;
        }
    }
}
