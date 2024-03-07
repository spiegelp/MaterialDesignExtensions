using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using MaterialDesignThemes.Wpf;

namespace MaterialDesignExtensions.Themes
{
    public class MaterialDesignExtendedLightTheme : MaterialDesignLightTheme, IExtendedBaseTheme
    {
        public Color MaterialDesignNavigationItemIcon { get; } = (Color)ColorConverter.ConvertFromString("#8A000000");

        public Color MaterialDesignNavigationItemText { get; } = (Color)ColorConverter.ConvertFromString("#DE000000");

        public Color MaterialDesignNavigationItemSubheader { get; } = (Color)ColorConverter.ConvertFromString("#8A000000");

        public Color MaterialDesignStepperInactiveStep { get; } = (Color)ColorConverter.ConvertFromString("#61000000");

        public Color MaterialDesignStepperActiveStep { get; } = (Color)ColorConverter.ConvertFromString("#DD000000");

        public Color MaterialDesignStepperSeparator { get; } = (Color)ColorConverter.ConvertFromString("#1F000000");

        public MaterialDesignExtendedLightTheme() : base() { }
    }
}
