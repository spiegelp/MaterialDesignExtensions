using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MaterialDesignExtensions.Converters
{
    public class DateTimeAgoConverter : IValueConverter
    {
        public DateTimeAgoConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                DateTime now = DateTime.Now;
                TimeSpan timeSpan = now - dateTime;

                if (timeSpan.TotalHours < 24)
                {
                    return dateTime.ToShortTimeString();
                }
                else
                {
                    return dateTime.ToShortDateString();
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
