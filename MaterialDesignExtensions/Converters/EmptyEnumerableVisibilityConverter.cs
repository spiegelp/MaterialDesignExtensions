using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MaterialDesignExtensions.Converters
{
    /// <summary>
    /// Maps a null or empty <see cref="IEnumerable" /> to a <see cref="Visibility" />.
    /// </summary>
    public class EmptyEnumerableVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// The visibility value of an empty or null <see cref="IEnumerable" />.
        /// </summary>
        public Visibility EmptyVisibility { get; set; }

        /// <summary>
        /// The visibility value of a non empty <see cref="IEnumerable" />.
        /// </summary>
        public Visibility NotEmptyVisibility { get; set; }

        /// <summary>
        /// Creates a new <see cref="EmptyEnumerableVisibilityConverter" />.
        /// </summary>
        public EmptyEnumerableVisibilityConverter()
        {
            EmptyVisibility = Visibility.Collapsed;
            NotEmptyVisibility = Visibility.Visible;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable enumerable)
            {
                return enumerable.GetEnumerator().MoveNext() ? NotEmptyVisibility : EmptyVisibility;
            }
            else
            {
                return Binding.DoNothing;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
