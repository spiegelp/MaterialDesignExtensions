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
    /// Converts a <see cref="ResizeMode" /> of a window into an enabled state of an according caption button.
    /// </summary>
    public class WindowCaptionButtonEnabledConverter : WindowCaptionButtonBaseConverter
    {
        /// <summary>
        /// Creates a new <see cref="WindowCaptionButtonEnabledConverter" />.
        /// </summary>
        public WindowCaptionButtonEnabledConverter() : base() { }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (values != null && values.Length == 2)
                {
                    string buttonName = (string)values[0];
                    ResizeMode resizeMode = (ResizeMode)values[1];

                    if (buttonName == CloseButtonName)
                    {
                        return true;
                    }
                    else if (buttonName == MinimizeButtonName)
                    {
                        return resizeMode != ResizeMode.NoResize;
                    }
                    else if (buttonName == MaximizeRestoreButtonName)
                    {
                        return resizeMode != ResizeMode.NoResize && resizeMode != ResizeMode.CanMinimize;
                    }
                }
            }
            catch (Exception)
            {
                // use the default return value below
            }

            return true;
        }
    }
}
