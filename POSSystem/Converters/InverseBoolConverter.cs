using Microsoft.UI.Xaml.Data;
using System;

namespace POSSystem.Converters
{
    /// <summary>
    /// Converts a boolean value to its inverse and vice versa.
    /// </summary>
    public class InverseBoolConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to its inverse.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The inverse of the boolean value.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolean)
            {
                return !boolean;
            }

            return value;
        }

        /// <summary>
        /// Converts a boolean value back to its inverse.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The inverse of the boolean value.</returns>
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