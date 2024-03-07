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
    /// Converter to apply boolean "and" operation and map to a <see cref="Visibility" />.
    /// </summary>
    public class BooleanOrToVisibilityConverter : IMultiValueConverter
    {
        private BooleanOrConverter m_booleanOrConverter;
        private BoolToVisibilityConverter m_boolToVisibilityConverter;

        /// <summary>
        /// The visibility value if the argument is false.
        /// </summary>
        public Visibility FalseValue
        {
            get
            {
                return m_boolToVisibilityConverter.FalseValue;
            }

            set
            {
                m_boolToVisibilityConverter.FalseValue = value;
            }
        }

        /// <summary>
        /// The visibility value if the argument is true.
        /// </summary>
        public Visibility TrueValue
        {
            get
            {
                return m_boolToVisibilityConverter.TrueValue;
            }

            set
            {
                m_boolToVisibilityConverter.TrueValue = value;
            }
        }

        /// <summary>
        /// Creates a new <see cref="BooleanOrToVisibilityConverter" />.
        /// </summary>
        public BooleanOrToVisibilityConverter()
        {
            m_booleanOrConverter = new BooleanOrConverter();
            m_boolToVisibilityConverter = new BoolToVisibilityConverter();
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = (bool)m_booleanOrConverter.Convert(values, targetType, parameter, culture);

            return m_boolToVisibilityConverter.Convert(b, targetType, parameter, culture);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
