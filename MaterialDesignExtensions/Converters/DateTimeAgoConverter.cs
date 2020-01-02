using System;
using System.Globalization;
using System.Windows.Data;

namespace MaterialDesignExtensions.Converters
{
    /// <summary>
    /// Converter for displaying a past <see cref="DateTime" />.
    /// </summary>
    public class DateTimeAgoConverter : IValueConverter
    {
        /// <summary>
        /// Creates a new <see cref="DateTimeAgoConverter" />.
        /// </summary>
        public DateTimeAgoConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                DateTime now = DateTime.Now;
                TimeSpan timeSpan = now - dateTime;

                if (now.Date == dateTime.Date && timeSpan.TotalHours < 24)
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
