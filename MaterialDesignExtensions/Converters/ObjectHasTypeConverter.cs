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
    /// Checks the type of the argument if it is of a specified type.
    /// </summary>
    public class ObjectHasTypeConverter : IMultiValueConverter
    {
        /// <summary>
        /// The fully qualified name of the type.
        /// </summary>
        public string FullTypeName { get; set; }

        /// <summary>
        /// Creates a new <see cref="ObjectHasTypeConverter" />.
        /// </summary>
        public ObjectHasTypeConverter() { }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length == 2)
            {
                string valueTypeFullName = values[0] != null ? values[0].GetType().FullName : "null";

                string typeFullName = null;

                if (values[1] is Type type)
                {
                    typeFullName = type.FullName;
                }
                else if (values[1] is string typeName)
                {
                    typeFullName = typeName;
                }

                return valueTypeFullName == typeFullName;
            }
            else
            {
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
