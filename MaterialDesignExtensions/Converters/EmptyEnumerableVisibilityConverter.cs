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
    public class EmptyEnumerableVisibilityConverter : IValueConverter
    {
        public Visibility EmptyVisibility { get; set; }

        public Visibility NotEmptyVisibility { get; set; }

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
