using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MaterialDesignExtensions.Converters
{
    /// <summary>
    /// Converts a <see cref="WindowStyle" /> and a <see cref="ResizeMode" /> of a window into a <see cref="Visibility" /> of an according caption button.
    /// </summary>
    public class WindowCaptionButtonVisibilityConverter : WindowCaptionButtonBaseConverter
    {
        /// <summary>
        /// Creates a new <see cref="WindowCaptionButtonVisibilityConverter" />.
        /// </summary>
        public WindowCaptionButtonVisibilityConverter() : base() { }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (values != null && values.Length == 3)
                {
                    string buttonName = (string)values[0];
                    WindowStyle windowStyle = (WindowStyle)values[1];
                    ResizeMode resizeMode = (ResizeMode)values[2];

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
                        if (resizeMode != ResizeMode.NoResize
                            && (windowStyle == WindowStyle.SingleBorderWindow || windowStyle == WindowStyle.ThreeDBorderWindow))
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
    }
}
