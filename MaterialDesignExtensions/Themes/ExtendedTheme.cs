using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using MaterialDesignThemes.Wpf;

namespace MaterialDesignExtensions.Themes
{
    public class ExtendedTheme : Theme, IExtendedTheme
    {
        public static IExtendedBaseTheme ExtendedLightTheme { get; } = new MaterialDesignExtendedLightTheme();

        public static IExtendedBaseTheme ExtendedDarkTheme { get; } = new MaterialDesignExtendedDarkTheme();

        public Color NavigationItemIcon { get; set; }

        public Color NavigationItemText { get; set; }

        public Color NavigationItemSubheader { get; set; }

        public Color StepperInactiveStep { get; set; }

        public Color StepperActiveStep { get; set; }

        public Color StepperSeparator { get; set; }

        public static ExtendedTheme Create(IExtendedBaseTheme baseTheme, Color primary, Color accent)
        {
            if (baseTheme == null)
            {
                throw new ArgumentNullException(nameof(baseTheme));
            }

            ExtendedTheme theme = new ExtendedTheme();

            theme.SetBaseTheme(baseTheme);
            theme.SetPrimaryColor(primary);
            theme.SetSecondaryColor(accent);

            return theme;
        }
    }
}
