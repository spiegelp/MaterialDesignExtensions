using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MaterialDesignExtensions.Converters
{
    public class BooleanOrConverter : IMultiValueConverter
    {
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
