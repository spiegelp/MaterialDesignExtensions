using System;
using System.Globalization;
using System.Windows.Data;

namespace MaterialDesignExtensions.Converters
{
    /// <summary>
    /// Converter to apply boolean "or" operation.
    /// </summary>
    public class BooleanOrConverter : IMultiValueConverter
    {
        /// <summary>
        /// Creates a new <see cref="BooleanOrConverter" />.
        /// </summary>
        public BooleanOrConverter() { }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;

            if (values != null)
            {
                for (int i = 0; i < values.Length && !result; i++)
                {
                    bool? b = values[i] as bool?;

                    if (b != null)
                    {
                        result = result || b.Value;
                    }
                }
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
