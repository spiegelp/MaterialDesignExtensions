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
    public class NullToVisibilityConverter : IValueConverter
    {
        public Visibility NullValue { get; set; }

        public Visibility NotNullValue { get; set; }

        public NullToVisibilityConverter()
        {
            NullValue = Visibility.Collapsed;
            NotNullValue = Visibility.Visible;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? NullValue : NotNullValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
