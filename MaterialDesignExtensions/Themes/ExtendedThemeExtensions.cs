using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MaterialDesignThemes.Wpf;

namespace MaterialDesignExtensions.Themes
{
    public static class ExtendedThemeExtensions
    {
        public static IExtendedBaseTheme GetExtendedBaseTheme(this BaseTheme baseTheme)
        {
            switch (baseTheme)
            {
                case BaseTheme.Dark:
                    return ExtendedTheme.ExtendedDarkTheme;

                case BaseTheme.Light:
                    return ExtendedTheme.ExtendedLightTheme;

                default:
                    throw new InvalidOperationException();
            }
        }

        public static void SetBaseTheme(this IExtendedTheme theme, IExtendedBaseTheme baseTheme)
        {
            // call implementation of MaterialDesignThemes
            theme.SetBaseTheme((IBaseTheme)baseTheme);

            // set extended properties of the theme
            theme.NavigationItemIcon = baseTheme.MaterialDesignNavigationItemIcon;
            theme.NavigationItemText = baseTheme.MaterialDesignNavigationItemText;
            theme.NavigationItemSubheader = baseTheme.MaterialDesignNavigationItemSubheader;
            theme.StepperInactiveStep = baseTheme.MaterialDesignStepperInactiveStep;
            theme.StepperActiveStep = baseTheme.MaterialDesignStepperActiveStep;
            theme.StepperSeparator = baseTheme.MaterialDesignStepperSeparator;
        }
    }
}
