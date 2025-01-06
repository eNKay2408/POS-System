using Microsoft.UI.Xaml.Data;
using System;

namespace POSSystem.Converters
{
    /// <summary>
    /// Converts a decimal value to a string with exactly 2 decimal places and vice versa.
    /// </summary>
    public class DecimalToStringConverter : IValueConverter
    {
        /// <summary>
        /// Converts a decimal value to a string with exactly 2 decimal places.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>A string representation of the decimal value with 2 decimal places.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is decimal decimalValue)
            {
                return decimalValue.ToString("F2"); // Format to 2 decimal places
            }else if (value is int  intValue)
            {
                //explicit conversion to decimal
                decimal d = intValue;
                return d.ToString("F2");
            }
            return null;
        }

        /// <summary>
        /// Converts a string back to a decimal value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>A decimal value parsed from the string, or null if parsing fails.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (decimal.TryParse(value as string, out var result))
            {
                return result;
            }
            return null;
        }
    }
}
