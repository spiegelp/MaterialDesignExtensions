using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MaterialDesignExtensions.Converters
{
    /// <summary>
    /// Converts a null value to a <see cref="Visibility" />.
    /// </summary>
    public class NullToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// The visibility value for a null value.
        /// </summary>
        public Visibility NullValue { get; set; }

        /// <summary>
        /// The visibility value for a not null value.
        /// </summary>
        public Visibility NotNullValue { get; set; }

        /// <summary>
        /// Creates a new <see cref="NullToVisibilityConverter" />.
        /// </summary>
        public NullToVisibilityConverter()
        {
            NullValue = Visibility.Collapsed;
            NotNullValue = Visibility.Visible;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? NullValue : NotNullValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
