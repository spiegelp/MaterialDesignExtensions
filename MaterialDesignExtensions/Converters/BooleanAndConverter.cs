using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace MaterialDesignExtensions.Converters
{
    /// <summary>
    /// Converter to apply boolean "and" operation.
    /// </summary>
    public class BooleanAndConverter : IMultiValueConverter
    {
        /// <summary>
        /// Creates a new <see cref="BooleanAndConverter" />.
        /// </summary>
        public BooleanAndConverter() { }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool[] booleanValues = values.Select(value => value as bool?)
                .Where(b => b != null)
                .Select(b => b.Value)
                .ToArray();

            if (booleanValues.Length > 0)
            {
                bool result = booleanValues[0];

                for (int i = 1; i < booleanValues.Length && result; i++)
                {
                    result = result && booleanValues[i];
                }

                return result;
            }
            else
            {
                return false;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
