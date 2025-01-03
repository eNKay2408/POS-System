using Microsoft.UI.Xaml.Data;
using System;

namespace POSSystem.Converters
{
    /// <summary>
    /// Converts an integer value to a string and vice versa.
    /// </summary>
    public class IntToStringConverter : IValueConverter
    {
        /// <summary>
        /// Converts an integer value to a string.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>A string representation of the integer value.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Converts a string back to an integer value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>An integer value parsed from the string, or null if parsing fails.</returns>
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
