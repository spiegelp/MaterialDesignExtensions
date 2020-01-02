using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MaterialDesignExtensions.Converters
{
    /// <summary>
    /// Converts an empty collection to a <see cref="Visibility" />.
    /// </summary>
    public class ObjectCollectionToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// The visibility value of an empty collection.
        /// </summary>
        public Visibility EmptyValue { get; set; }

        /// <summary>
        /// Creates a new <see cref="ObjectCollectionToVisibilityConverter" />.
        /// </summary>
        public ObjectCollectionToVisibilityConverter()
        {
            EmptyValue = Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null
                    && ((value is ICollection && ((ICollection)value).Count > 0)
                        || (value is Collection<object> && ((Collection<object>)value).Count > 0)))
            {
                return Visibility.Visible;
            }
            else
            {
                return EmptyValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
