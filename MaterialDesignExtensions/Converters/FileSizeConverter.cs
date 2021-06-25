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
    /// Converts a file size of bytes into a user friendly string.
    /// </summary>
    public class FileSizeConverter : IValueConverter
    {
        /// <summary>
        /// Creates a new <see cref="FileSizeConverter" />.
        /// </summary>
        public FileSizeConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long length = -1;

            if (value is long)
            {
                length = (long)value;
            }
            else if (value is int)
            {
                length = (int)value;
            }

            if (length > -1)
            {
                double lengthBytes = length;
                double lengthKB = lengthBytes / 1000;
                double lengthMB = lengthKB / 1000;
                double lengthGB = lengthMB / 1000;

                if (lengthGB >= 1.0)
                {
                    return lengthGB.ToString("0.#") + " GB";
                }
                else if (lengthMB >= 1.0)
                {
                    return lengthMB.ToString("0.#") + " MB";
                }
                else if (lengthKB >= 1.0)
                {
                    return lengthKB.ToString("0.#") + " KB";
                }
                else
                {
                    return length + " Bytes";
                }
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
