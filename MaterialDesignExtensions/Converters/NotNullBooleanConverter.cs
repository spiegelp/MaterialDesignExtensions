using System;
using System.Globalization;
using System.Windows.Data;

namespace MaterialDesignExtensions.Converters
{
    /// <summary>
    /// Converts a not null value to true, otherwise false.
    /// A special logic for strings will be applied to handle empty or whitespace only strings like null values.
    /// </summary>
    public class NotNullBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Creates a new <see cref="NotNullBooleanConverter" />.
        /// </summary>
        public NotNullBooleanConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                return !string.IsNullOrWhiteSpace(str);
            }
            else
            {
                return value != null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
