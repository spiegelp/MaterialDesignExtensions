using System;
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
    /// Converter to map a boolean to a <see cref="Visibility" />.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// The visibility value if the argument is false.
        /// </summary>
        public Visibility FalseValue { get; set; }

        /// <summary>
        /// The visibility value if the argument is true.
        /// </summary>
        public Visibility TrueValue { get; set; }

        /// <summary>
        /// Creates a new <see cref="BoolToVisibilityConverter" />.
        /// </summary>
        public BoolToVisibilityConverter()
        {
            FalseValue = Visibility.Collapsed;
            TrueValue = Visibility.Visible;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool && ((bool)value) == false)
            {
                return FalseValue;
            }
            else
            {
                return TrueValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
