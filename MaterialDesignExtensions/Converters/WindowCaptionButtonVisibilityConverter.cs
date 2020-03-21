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
    /// Converts a <see cref="WindowStyle" /> of a window into a <see cref="Visibility" /> of an according caption button.
    /// </summary>
    public class WindowCaptionButtonVisibilityConverter : IMultiValueConverter
    {
        /// <summary>
        /// Identifier for the minimize caption button.
        /// </summary>
        public string MinimizeButtonName => "minimizeButton";

        /// <summary>
        /// Identifier for the maximize/restore caption button.
        /// </summary>
        public string MaximizeRestoreButtonName => "maximizeRestoreButton";

        /// <summary>
        /// Identifier for the close caption button.
        /// </summary>
        public string CloseButtonName => "closeButton";

        /// <summary>
        /// Creates a new <see cref="WindowCaptionButtonVisibilityConverter" />.
        /// </summary>
        public WindowCaptionButtonVisibilityConverter() { }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (values != null && values.Length == 2)
                {
                    string buttonName = (string)values[0];
                    WindowStyle windowStyle = (WindowStyle)values[1];

                    if (buttonName == CloseButtonName)
                    {
                        if (windowStyle != WindowStyle.None)
                        {
                            return Visibility.Visible;
                        }
                        else
                        {
                            return Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        if (windowStyle == WindowStyle.SingleBorderWindow || windowStyle == WindowStyle.ThreeDBorderWindow)
                        {
                            return Visibility.Visible;
                        }
                        else
                        {
                            return Visibility.Collapsed;
                        }
                    }
                }
            }
            catch (Exception)
            {
                // use the default return value below
            }

            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
