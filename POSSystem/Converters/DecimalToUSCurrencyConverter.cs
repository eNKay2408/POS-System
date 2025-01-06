using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.Converters
{
    /// <summary>
    /// Converts a decimal value to a US currency string and vice versa.
    /// </summary>
    public class DecimalToUSCurrencyConverter : IValueConverter
    {
        /// <summary>
        /// Converts a decimal value to a US currency string.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>A string representation of the decimal value formatted as US currency.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is decimal decimalValue)
            {
                return decimalValue.ToString("C2", CultureInfo.GetCultureInfo("en-US")); // Format as US currency
            }
            return null;
        }

        /// <summary>
        /// Converts a US currency string back to a decimal value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>A decimal value parsed from the US currency string, or null if parsing fails.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (decimal.TryParse(value as string, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"), out var result))
            {
                return result;
            }
            return null;
        }
    }
}
