using System.Windows.Media;

using MaterialDesignThemes.Wpf;

namespace MaterialDesignExtensions.Themes
{
    public interface IExtendedBaseTheme : IBaseTheme
    {
        Color MaterialDesignNavigationItemIcon { get; }

        Color MaterialDesignNavigationItemText { get; }

        Color MaterialDesignNavigationItemSubheader { get; }

        Color MaterialDesignStepperInactiveStep { get; }

        Color MaterialDesignStepperActiveStep { get; }

        Color MaterialDesignStepperSeparator { get; }
    }
}
