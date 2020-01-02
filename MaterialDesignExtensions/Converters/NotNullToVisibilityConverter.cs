using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MaterialDesignExtensions.Converters
{
    /// <summary>
    /// Converts null or not null to a <see cref="Visibility" />.
    /// </summary>
    public class NotNullToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// The visibility value if the argument is null.
        /// </summary>
        public Visibility NullValue { get; set; }

        /// <summary>
        /// Creates a new <see cref="NotNullToVisibilityConverter" />.
        /// </summary>
        public NotNullToVisibilityConverter()
        {
            NullValue = Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? NullValue : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
