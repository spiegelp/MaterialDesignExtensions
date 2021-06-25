using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MaterialDesignExtensions.Converters
{
    /// <summary>
    /// Converts an object to its fully qualified type name.
    /// </summary>
    public class ObjectToTypeStringConverter : IValueConverter
    {
        /// <summary>
        /// Creates a new <see cref="ObjectToTypeStringConverter" />.
        /// </summary>
        public ObjectToTypeStringConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return value.GetType().FullName;
            }
            else
            {
                return "null";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // not used yet
            return Binding.DoNothing;
        }
    }
}
